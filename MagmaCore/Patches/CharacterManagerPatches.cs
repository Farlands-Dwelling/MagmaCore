using MagmaCore.Customs;
using MagmaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using MagmaCore.Managers;
using MelonLoader;

namespace MagmaCore.Patches
{
    public static class CharacterManagerHelper
    {
        public static void InvokeForMainCharacter(Action<object> action, string methodName)
        {
            foreach (MonoBehaviour manager in Main.Managers)
            {
                if (manager == null) continue;

                Type componentType = manager.GetType().BaseType;

                // Check if the component inherits from CustomCharacterManager<> (generic base type)
                if (componentType.IsGenericType && componentType.GetGenericTypeDefinition() == typeof(CustomCharacterManager<>))
                {
                    // Check if the characterName matches
                    var characterNameProperty = componentType.GetProperty("characterName");
                    var characterName = (Character.CharacterName)characterNameProperty?.GetValue(manager);
                    if (characterName == Player.main.characterName)
                    {
                        // Dynamically call the method passed as the action (e.g., StartCombat, EndCombat, etc.)
                        var method = componentType.GetMethod(methodName);
                        method?.Invoke(manager, null);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(PetMaster), nameof(PetMaster.StartCombat))]
    class StartCombatPatch
    {
        static void Postfix()
        {
            CharacterManagerHelper.InvokeForMainCharacter(characterManager => { }, "StartCombat");
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.EndBattle))]
    class EndCombatPatch
    {
        static void Postfix()
        {
            CharacterManagerHelper.InvokeForMainCharacter(characterManager => { }, "EndCombat");
        }
    }

    [HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndTurn))]
    class EndTurnPatch
    {
        static void Postfix()
        {
            CharacterManagerHelper.InvokeForMainCharacter(characterManager => { }, "EndTurn");
        }
    }

    [HarmonyPatch(typeof(TutorialManager), nameof(TutorialManager.TutorialTurnStart))]
    class TurnStartPatch
    {
        static void Postfix()
        {
            CharacterManagerHelper.InvokeForMainCharacter(characterManager => { }, "StartTurn");
        }
    }

    [HarmonyPatch(typeof(Tote), nameof(Tote.SpawnTote))]
    class SpawnPatch
    {
        static void Postfix()
        {
            CharacterManagerHelper.InvokeForMainCharacter(characterManager => { }, "Spawn");
        }
    }

    [HarmonyPatch(typeof(Tote), nameof(Tote.RemoveToteUI))]
    class RemoveUIPatch
    {
        static void Postfix()
        {
            CharacterManagerHelper.InvokeForMainCharacter(characterManager => { }, "RemoveUI");
        }
    }
}
