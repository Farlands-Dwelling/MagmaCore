using HarmonyLib;
using MagmaCore.Managera;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Load))]
    internal class FixBagSpritesPatch
    {
        static void Postfix()
        {
            MagmaManager.main.StartCoroutine(MagmaManager.main.FixBagCoroutine());
        }
            

    }
}
