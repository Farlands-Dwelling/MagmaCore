using HarmonyLib;
using MagmaCore.Customs;
using MagmaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaCore.Patches
{
    
    [HarmonyPatch(typeof(Player), "Awake")]
    class AddCharactersPatch
    {
        static bool Prefix(ref Player __instance)
        {
            List<Character> characterList = new List<Character>();
            foreach (CustomCharacter character in CustomUtils.GetCustomsOfType<CustomCharacter>())//CustomCharacter.CustomCharacters.Values)
            {
                characterList.Add(character.CharacterInstance);
            }
            __instance.characterProperties.AddRange(characterList);
            return true;
        }
    }
}
