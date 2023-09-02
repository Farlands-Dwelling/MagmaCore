using MagmaCore.Customs;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaCore.Utils
{
    // Credit goes to StarFluxGames and KitchenLib for the general "Custom" layout: https://github.com/KitchenMods/KitchenLib/blob/0e4660f10d9b864b279f2441b5c3ed1ed57ae50c/KitchenLib/src/Utils/GDOUtils.cs
    public static class CustomUtils
    {
        public static CustomBase GetCustom(string modID, string name)
        {
            CustomBase result;
            CustomBase.CustomsByGUID.TryGetValue(new KeyValuePair<string, string>(modID, name), out result);
            if (result == null)
            {
                CustomBase.CustomsByModName.TryGetValue(new KeyValuePair<string, string>(modID, name), out result);
                MelonLogger.Warning($"Mod Name {modID}:{name} should not be used to find GDOs. Use Mod ID instead.");
            }
            return result;
        }
        public static CustomBase GetCustom(int id)
        {
            CustomBase.Customs.TryGetValue(id, out var result);
            return result;
        }
        public static CustomBase GetCustom<T>()
        {
            CustomBase.CustomsByType.TryGetValue(typeof(T), out var result);
            return result;
        }

        public static T GetCastedCustom<T>(string modID, string name) where T : CustomBase
        {
            return (T)GetCustom(modID, name);
        }
    }
}
