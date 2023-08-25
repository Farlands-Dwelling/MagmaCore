using MagmaCore.Customs;
using MagmaCore.Datatypes;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Character;
using static UnityEngine.Rendering.DebugUI;

namespace MagmaCore.Utils
{
    public static class ItemUtils
    {
        public static Dictionary<int, Item2.ItemType> CustomItemTypes = new Dictionary<int, Item2.ItemType>();
        public static Dictionary<KeyValuePair<string, string>, Item2.ItemType> CustomItemTypesByModName = new Dictionary<KeyValuePair<string, string>, Item2.ItemType>();

        public static Item2 FindItem(string itemName)
        {
            ModItemLoader.main.gameItemLookup.TryGetValue("bph###" + itemName.ToLower(), out var foundItem);
            if (foundItem == null)
            {
                MelonLogger.Error($"Could not find item {itemName}.");
            }
            else if (foundItem.GetComponent<Item2>() == null)
            {
                MelonLogger.Error($"Could not find Item2 component on 'item' {itemName}.");
            }
            return foundItem.GetComponent<Item2>();
        }

        public static Item2 FindItem(string itemName, string modpackName)
        {
            bool foundModpack = ModpackUtils.GetModpackFromInternalName(modpackName) != null;

            ModItemLoader.main.modItemLookup.TryGetValue($"{modpackName.ToLower()}###" + itemName.ToLower(), out var foundItem);

            if (foundItem == null)
            {
                MelonLogger.Error($"Could not find item {itemName}.");
            }
            if (!foundModpack)
            {
                MelonLogger.Error($"Could not find modpack {modpackName} when trying to load item {itemName}.");
            }
            return foundItem;
        }

        public static Item2 FindItem(ModItemDefinition itemDef)
        {
            return FindItem(itemDef.itemName, itemDef.internalModpackName);
        }

        public static Item2.ItemType RegisterItemType(string name, MelonInfoAttribute modInfo)
        {
            // TODO: Make work with any enum.

            int id = StringUtils.GetInt32HashCode($"{modInfo.Name}:{name}");

            Item2.ItemType newValue = (Item2.ItemType)id;

            CustomItemTypes.Add(id, newValue);
            CustomItemTypesByModName.Add(new KeyValuePair<string, string>(modInfo.Name, name), newValue);

            TranslationUtils.CreateTranslation(id.ToString(), name);

            return newValue;
        }

        public static Item2.ItemType RegisterItemType(string name, string modName)
        {
            // TODO: Make work with any enum.

            int id = StringUtils.GetInt32HashCode($"{modName}:{name}");

            Item2.ItemType newValue = (Item2.ItemType)id;

            CustomItemTypes.Add(id, newValue);
            CustomItemTypesByModName.Add(new KeyValuePair<string, string>(modName, name), newValue);

            TranslationUtils.CreateTranslation(id.ToString(), name);

            return newValue;
        }

        // Returns first matching item found
        public static Item2 FindItemInInventory(Item2 itemToFind)
        {
            Item2 firstMatch = GameObject.FindObjectsOfType<Item2>().ToList().Find(x => Item2.GetDisplayName(x.name) == Item2.GetDisplayName(itemToFind.name) && !x.destroyed);

            return firstMatch;
        }

        public static List<Item2> FindItemsInInventory(Item2 itemToFind)
        {
            List<Item2> matches = GameObject.FindObjectsOfType<Item2>().ToList().FindAll(x => Item2.GetDisplayName(x.name) == Item2.GetDisplayName(itemToFind.name) && !x.destroyed);

            return matches;
        }
    }
}
