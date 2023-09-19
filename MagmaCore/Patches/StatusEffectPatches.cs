using HarmonyLib;
using MagmaCore.Customs;
using MagmaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(StatusEffect), nameof(StatusEffect.Start))]
    class StatusEffectStartPatch
    {
        static void Postfix(StatusEffect __instance)
        {
            foreach (CustomStatusEffect statusEffect in CustomUtils.GetCustomsOfType<CustomStatusEffect>())
            {
                if (__instance.type == statusEffect.StatusEffectInstance)
                {
                    __instance.isNumeric = statusEffect.IsNumeric;
                    __instance.decreasesTimeType = statusEffect.DecreasesTimeType;
                }
            }

            __instance.UpdateValue();
        }
    }

    [HarmonyPatch(typeof(StatusEffect), nameof(StatusEffect.GetSpriteFromType))]
    class ApplyStatusEffectSpritePatch
    {
        static void Postfix(ref Sprite __result, StatusEffect __instance, StatusEffect.Type type)
        {
            foreach (CustomStatusEffect statusEffect in CustomUtils.GetCustomsOfType<CustomStatusEffect>())
            {
                if (type == statusEffect.StatusEffectInstance)
                {
                    __result = statusEffect.Sprite;
                }
            }
        }
    }

    [HarmonyPatch(typeof(StatusEffect), nameof(StatusEffect.GetNameKeyFromType))]
    class GetNameKeyFromTypePatch
    {
        static void Postfix(ref string __result, StatusEffect __instance, StatusEffect.Type type)
        {
            foreach (CustomStatusEffect statusEffect in CustomUtils.GetCustomsOfType<CustomStatusEffect>())
            {
                if (type == statusEffect.StatusEffectInstance)
                {
                    __result = statusEffect.Translations.TranslationKey;
                }
            }
        }
    }
}
