using MagmaCore.Customs;
using MagmaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using HarmonyLib;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(EventButton), nameof(EventButton.FullEffect))]
    class FullEffectPatch
    {
        static void Postfix(ref EventButton __instance, DungeonEvent dungeonEvent)
        {
            foreach (CustomEventButton customEventButton in CustomUtils.GetCustomsOfType<CustomEventButton>())
            {
                if (customEventButton.Instance == __instance)
                {
                    customEventButton.OnClick();
                }
            }
        }
    }
}
