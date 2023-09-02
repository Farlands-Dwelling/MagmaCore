using HarmonyLib;
using MagmaCore.Customs;
using MagmaCore.Utils;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(Singleton), nameof(Singleton.SetNumberFromCharacter))]
    class SetNumberFromCharacterPatch
    {
        static void Postfix(ref Singleton __instance, Character.CharacterName name)
        {
            foreach (KeyValuePair<int, CustomCharacter> characterKvp in CustomUtils.GetCustomsOfTypeWithID<CustomCharacter>())
            {
                int paddingAmount = Resources.FindObjectsOfTypeAll<Character>().Length - CustomUtils.GetCustomsOfType<CustomCharacter>().Count;
                if (name == characterKvp.Value.CharacterInstance.characterName)
                {
                    MelonLogger.Warning("Custom char detected 2: " + characterKvp.Value.characterName);
                    __instance.characterNumber = CustomUtils.GetCustomsOfTypeWithID<CustomCharacter>().Values.ToList().IndexOf(characterKvp.Value) + paddingAmount;
                }
            }
        }
    }
}
