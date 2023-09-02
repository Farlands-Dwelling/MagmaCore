using HarmonyLib;
using MagmaCore.Customs;
using MagmaCore.Datatypes;
using MagmaCore.Utils;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(Player), nameof(Player.SpawnObjects))]
    class AddStartingObjectsPatch
    {
        static bool Prefix()
        {
            foreach (CustomCharacter character in CustomUtils.GetCustomsOfType<CustomCharacter>())// CustomCharacter.CustomCharacters.Values)
            {
                if (character.startingItems == null)
                {
                    return true;
                } 
                else
                {
                    character.CharacterInstance.startingObjects.Clear(); 
                }

                foreach (ModItemDefinition itemDef in character.startingItems)
                {
                    Item2 item = null;
                    if (itemDef.internalModpackName == null)
                        item = ItemUtils.FindItem(itemDef.itemName);
                    else
                        item = ItemUtils.FindItem(itemDef);

                    if (item != null)
                        character.CharacterInstance.startingObjects.Add(item.gameObject);
                }
            }
             
            return true;
        }
    }

    /*    [HarmonyPatch(typeof(MenuManager), "GoAdventuring")]
        internal class DependencyCheckStartGamePatch
        {
            static bool Prefix(ref MenuManager __instance)
            {
                // Add code that checks if all dependencies are present, if not then it transitions to special "dependencies" scene + return false. If they are, return true.
                return true;
            }
        }*/
}
