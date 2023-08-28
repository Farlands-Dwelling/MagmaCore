using HarmonyLib;
using MagmaCore.Customs;
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
            foreach (KeyValuePair<int, CustomCharacter> characterKvp in CustomCharacter.CustomCharacters)
            {
                int paddingAmount = Resources.FindObjectsOfTypeAll<Character>().Length - CustomCharacter.CustomCharacters.Count;
                if (name == characterKvp.Value.CharacterInstance.characterName)
                {
                    MelonLogger.Warning("Custom char detected 2: " + characterKvp.Value.characterName);
                    __instance.characterNumber = CustomCharacter.CustomCharacters.Values.ToList().IndexOf(characterKvp.Value) + paddingAmount;
                }
            }
        }
    }
}
