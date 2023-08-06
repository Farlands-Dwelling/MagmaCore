using HarmonyLib;
using MagmaCore.Customs;
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
            __instance.characterProperties.AddRange(CustomCharacter.Characters);
            return true;
        }
    }
}
