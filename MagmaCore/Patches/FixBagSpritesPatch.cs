using HarmonyLib;
using MagmaCore.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(ItemPouch), nameof(ItemPouch.SetAllItemsToItemsParent))]
    internal class FixBagSpritesPatch
    {
        static void Postfix()
        {
            MagmaManager.main.StartCoroutine(MagmaManager.main.FixBagCoroutine());
        }
            

    }
}
