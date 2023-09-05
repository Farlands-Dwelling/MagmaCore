using HarmonyLib;
using MagmaCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(ContextMenuManager), nameof(ContextMenuManager.Command))]
    internal class ContextMenuTypePatch
    {
        static void Postfix(ref ContextMenuManager __instance, ContextMenuButton.Type type, List<Item2.Cost> costs, Item2.PlayerAnimation playerAnimation)
        {
            __instance.selectedItem.GetComponent<IExtraItemFunction>().Event(__instance, type);

            return;
        }
    }
}
