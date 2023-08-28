using HarmonyLib;
using MagmaCore.Customs;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MelonLoader.MelonLogger;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(Singleton), nameof(Singleton.SetCharacterFromNumber))]
    class SetCharacterFromNumberPatch
    {
        static bool Prefix(ref Singleton __instance)
        {
            if (Singleton.Instance.characterNumber < 0 || Singleton.Instance.characterNumber > Resources.FindObjectsOfTypeAll<Character>().Length)
            {
                Singleton.Instance.characterNumber = 0;
            }

            if (__instance.characterNumber == 0)
            {
                __instance.character = Character.CharacterName.Purse;
            }
            if (__instance.characterNumber == 1)
            {
                __instance.character = Character.CharacterName.Tote;
            }
            if (__instance.characterNumber == 2)
            {
                __instance.character = Character.CharacterName.CR8;
            }
            if (__instance.characterNumber == 3)
            {
                __instance.character = Character.CharacterName.Satchel;
            }
            if (__instance.characterNumber == 4)
            {
                __instance.character = Character.CharacterName.Pochette;
            }
            foreach (KeyValuePair<int, CustomCharacter> characterKvp in CustomCharacter.CustomCharacters)
            {
                int paddingAmount = Resources.FindObjectsOfTypeAll<Character>().Length - CustomCharacter.CustomCharacters.Count;
                MelonLogger.Msg(Resources.FindObjectsOfTypeAll<Character>().Length - CustomCharacter.CustomCharacters.Count);
                if (__instance.characterNumber == CustomCharacter.CustomCharacters.Values.ToList().IndexOf(characterKvp.Value) + paddingAmount)
                {
                    MelonLogger.Warning("Custom char detected: " + characterKvp.Value.characterName);
                    __instance.character = (Character.CharacterName)characterKvp.Key;
                }
            }

            return false;
        }
    }
}
