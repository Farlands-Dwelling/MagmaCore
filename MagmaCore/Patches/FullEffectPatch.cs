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
using MelonLoader;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(EventButton), nameof(EventButton.FullEffect))]
    class FullEffectPatch
    {
        static void Postfix(ref EventButton __instance, DungeonEvent dungeonEvent)
        {
            MelonLogger.Msg(" ----- Heya ------ ");
            foreach (CustomEventButton customEventButton in CustomUtils.GetCustomsOfType<CustomEventButton>())
            {
                MelonLogger.Msg(" ----- 1 ------ ");
                foreach (PossibleOutcomeWithAction possibleOutcomeWithAction in customEventButton.possibleOutcomes)
                {
                    MelonLogger.Msg(" ----- 2 ------ ");
                    if (possibleOutcomeWithAction.possibleOutcome == __instance.chosenOutCome)
                    {
                        MelonLogger.Msg(" ----- 3 ------ ");
                        possibleOutcomeWithAction.OnClick?.Invoke();
                    }
                }
            }
        }
    }
}
