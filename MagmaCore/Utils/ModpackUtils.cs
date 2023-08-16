using MagmaCore.Patches;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Utils
{
    public static class ModpackUtils
    {
        public static bool CanRun = GameObject.FindObjectOfType<ModLoader>() != null;

        public static ModLoader.ModpackInfo GetModpackFromInternalName(string internalModpackName)
        {
            /*if (!CanRun)
            {
                MelonLogger.Warning($"Could not find ModLoader when trying to load modpack \"{internalModpackName}\"");
                return null;
            }*/
            ModLoader.ModpackInfo modpack = ModLoader.main.modpacks.Find(x => x.internalName.ToLower() == internalModpackName.ToLower());
            if (modpack == null)
            {
                MelonLogger.Warning($"Could not find modpack with the internal name \"{internalModpackName}\"");
            }
            return modpack;
        }

        public static ModLoader.ModpackInfo GetModpackFromDisplayName(string displayModpackName)
        {
            /*if (!CanRun)
            {
                MelonLogger.Warning($"Could not find ModLoader when trying to load modpack \"{displayModpackName}\"");
                return null;
            }*/
            ModLoader.ModpackInfo modpack = ModLoader.main.modpacks.Find(x => x.displayName.ToLower() == displayModpackName.ToLower());
            if (modpack == null)
            {
                MelonLogger.Warning($"Could not find modpack with the display name \"{displayModpackName}\"");
            }
            return modpack;
        }
    }
}
