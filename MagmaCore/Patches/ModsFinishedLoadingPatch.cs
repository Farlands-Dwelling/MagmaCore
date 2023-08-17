using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace MagmaCore.Patches
{
    /*[HarmonyPatch(typeof(ModItemLoader), nameof(ModLoader.Awake))]
    public static class ModsFinishedLoadingPatch
    {
        public static event Action OnLoadedMods;
        static void Postfix()
        {
            OnLoadedMods?.Invoke();
        }
    }*/
}
