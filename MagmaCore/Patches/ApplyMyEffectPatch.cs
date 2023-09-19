using HarmonyLib;
using MagmaCore.Customs;
using MagmaCore.Managers;
using MagmaCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(Item2), nameof(Item2.ApplyMyEffect))]
    class ApplyMyEffectPatch
    {
        static void Postfix(ref Item2 __instance, Item2.Effect effect, List<Status> stats, Status statsOfUser, Item2 applyingItem, Player player)
        {
            foreach (Status status in stats)
            {
                if (status != null)
                {
                    foreach (CustomStatusEffect statusEffect in CustomUtils.GetCustomsOfType<CustomStatusEffect>())
                    {
                        if (effect.type == statusEffect.Item2EffectInstance)
                        {
                            status.AddStatusEffect(statusEffect.StatusEffectInstance, effect.value, effect.mathematicalType);
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Enemy), nameof(Enemy.ApplyEffect))]
    class ApplyEffectPatch
    {
        static void Postfix(ref IEnumerator __result, ref Enemy __instance, Item2.CombattEffect combattEffect)
        {
            Enemy instance = __instance;
            Action postfixAction = () =>
            {
                // TERRIBLE way to do this, but I really don't want to do a transpiler patch
                if (instance.dead)
                {
                    return;
                }
                List<Status> list = new List<Status>();
                if (combattEffect.effect.target == Item2.Effect.Target.player)
                {
                    List<Status> list2 = new List<Status>();
                    list2.Add(UnityEngine.Object.FindObjectOfType<Player>().stats);
                    list = list2;
                    list = list2;
                }
                else if (combattEffect.effect.target == Item2.Effect.Target.enemy)
                {
                    list = new List<Status>
                        {
                            instance.stats
                        };
                }
                else if (combattEffect.effect.target == Item2.Effect.Target.allEnemies)
                {
                    foreach (Enemy enemy in Enemy.allEnemies)
                    {
                        if (!enemy.dead)
                        {
                            list.Add(enemy.stats);
                        }
                    }
                }
                List<Enemy> list3 = new List<Enemy>(Enemy.allEnemies);
                List<Enemy> list4 = new List<Enemy>();
                for (int i = 0; i < list3.Count; i++)
                {
                    Enemy enemy2 = list3[i];
                    if (!enemy2 || enemy2.dead || !enemy2.stats || enemy2.stats.IsCharmed())
                    {
                        list4.Add(list3[i]);
                        list3.RemoveAt(i);
                        i--;
                    }
                }
                if (instance.stats.IsCharmed())
                {
                    if (combattEffect.effect.target == Item2.Effect.Target.player)
                    {
                        if (list3.Count > 0)
                        {
                            int index = UnityEngine.Random.Range(0, list3.Count);
                            list = new List<Status>
                                {
                                    list3[index].stats
                                };
                        }
                    }
                    else if (combattEffect.effect.target == Item2.Effect.Target.allEnemies)
                    {
                        list = new List<Status>
                            {
                                instance.player.stats
                            };
                        foreach (Enemy enemy3 in list4)
                        {
                            list.Add(enemy3.stats);
                        }
                    }
                }
                foreach (Status status in list)
                {
                    #region Custom
                    foreach (CustomStatusEffect statusEffect in CustomUtils.GetCustomsOfType<CustomStatusEffect>())
                    {
                        if (combattEffect.effect.type == statusEffect.Item2EffectInstance)
                        {
                            status.AddStatusEffect(statusEffect.StatusEffectInstance, (float)Mathf.RoundToInt(combattEffect.effect.value), combattEffect.effect.mathematicalType);
                        }
                    }
                    #endregion
                }
            };
            var patchedEnumerator = new HarmonyUtils.PatchedEnumerator()
            {
                enumerator = __result,
                postfixAction = postfixAction,
            };
            __result = patchedEnumerator.GetEnumerator();
        }
    }

    //still need to patch Enemy.Turn.. but I reeeally don't want to
}
