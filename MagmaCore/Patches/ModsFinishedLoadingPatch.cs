using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(ModLoader), nameof(ModLoader.Start))]
    public static class ModsFinishedLoadingPatch
    {
        public static event Action OnLoadedMods;
        static void Postfix()
        {
            OnLoadedMods?.Invoke();
        }
    }
}
