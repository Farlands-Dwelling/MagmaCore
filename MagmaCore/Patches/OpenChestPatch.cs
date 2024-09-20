using MagmaCore.Customs;
using MagmaCore.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Patches
{
    [HarmonyLib.HarmonyPatch(typeof(Chest), nameof(Chest.OpenChest))]
    internal class OpenChestPatch
    {
        static bool Prefix(ref Chest __instance)
        {
            __instance.isOpen = true;
            TutorialManager tutorialManager = Object.FindObjectOfType<TutorialManager>();
            Player player = Object.FindObjectOfType<Player>();

            if (__instance.gameManager.floor != 0 || !tutorialManager || (tutorialManager.playType != TutorialManager.PlayType.tutorial && tutorialManager.playType != TutorialManager.PlayType.cr8Tutorial && tutorialManager.playType != TutorialManager.PlayType.totetorial))
            {
                foreach (CustomChest cChest in CustomUtils.GetCustomsOfType<CustomChest>())//CustomCharacter.CustomCharacters.Values)
                {
                    if (__instance.type == cChest.ChestInstance.type)
                    {
                        if (__instance.dungeonEvent)
                        {
                            __instance.dungeonEvent.FinishEvent();
                        }
                        if (__instance.chestParticles)
                        {
                            Object.Instantiate<GameObject>(__instance.chestParticles, __instance.transform.position + Vector3.up * 0.1f, Quaternion.identity);
                        }
                        SoundManager.main.PlaySFX("openChest");
                        __instance.GetComponent<SpriteRenderer>().sprite = __instance.openSprite;

                        if (cChest.useCustomOpenMethod)
                        {
                            cChest.OnOpen();
                            return false;
                        }

                        List<ItemSpawner.ItemToSpawn> items = cChest.chestItems;
                        int count = items.Count;
                        List<GameObject> list = new List<GameObject>();
                        for (int i = 0; i < count; i++)
                        {
                            GameObject gameObject = Object.Instantiate<GameObject>(items[i].item.gameObject);

                            // My guess is this math is used to calculate where items should be placed when spawned, taking into consideration how many items are spawned and offsetting the height to create rows
                            float y = -5f;
                            float x = ((float)i - (float)(count - 1) / 2f) * 1.5f;
                            if (count > 5)
                            {
                                if (i < 4)
                                {
                                    x = ((float)i - 1.5f) * 1.5f;
                                    y = -4f;
                                }
                                else
                                {
                                    x = ((float)(i - 4) - (float)(count - 4 - 1) / 2f) * 1.5f;
                                    y = -6f;
                                }
                            }
                            gameObject.transform.localPosition = new Vector3(x, y, 0f);

                            ItemMovement component = gameObject.GetComponent<ItemMovement>();
                            if (component)
                            {
                                component.outOfInventoryPosition = gameObject.transform.localPosition;
                                component.outOfInventoryRotation = Quaternion.identity;
                                component.returnsToOutOfInventoryPosition = true;
                                list.Add(gameObject);
                            }
                        }
                        __instance.gameManager.StartSimpleLimitedItemGetPeriod(1);
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
