using HarmonyLib;
using MagmaCore.Customs;
using MelonLoader;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(ModItemLoader), nameof(ModItemLoader.LoadItemFromFile))]
    class LoadModItemPatch
    {
        static void Postfix(ref GameObject __result, ref ModItemLoader __instance, ModLoader.ModpackInfo modpack, string path)
        {
            GameObject gameObject = __result;
            string directoryName = Path.GetDirectoryName(path) + "/";
            string jsonContents;
            try
            {
                jsonContents = File.ReadAllText(path);
            }
            catch (Exception inner)
            {
                throw new ModUtils.LoadingException(path, inner);
            }

            JObject jobject;
            try
            {
                jobject = JObject.Parse(jsonContents);
            }
            catch (Exception inner2)
            {
                throw new ModUtils.ParseException(path, inner2);
            }
            ModdedItem moddedItem = gameObject.GetComponent<ModdedItem>();

            Item2 item = gameObject.GetComponent<Item2>();
            SpriteRenderer component4 = gameObject.GetComponent<SpriteRenderer>();
            string text4 = "";
            string internalName = modpack.internalName;

            foreach (ItemLoaderExtension extension in ItemLoaderExtension.Extensions)
            {
                if (extension != null)
                    extension.Extension(ref gameObject, ref __instance, jobject);
            }
        }
    }
}
