using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Utils
{
    public static class ItemLoaderUtils
    {
        public static Item2 GetItem(JToken jtoken, Item2 item)
        {
            string text = "";
            if (jtoken.Type == JTokenType.String)
            {
                string str = "BPH###";
                //JToken jtoken = jtoken;
                text = str + ((jtoken != null) ? jtoken.ToString() : null);
            }
            else if (jtoken.Type == JTokenType.Object)
            {
                JObject jobject = (JObject)jtoken;
                if (!jobject.HasValues)
                {
                    return null;
                }
                if (jobject.Count > 1)
                {
                    throw new ModUtils.SyntaxException("MODPACK:ITEM Definition in Create Effect cannot have more than one item inside object. Make seperate objects for each item inside the array.");
                }
                JProperty jproperty = (JProperty)((JObject)jtoken).First;
                if (jobject[jproperty.Name].Type != JTokenType.String)
                {
                    throw new ModUtils.SyntaxException("MODPACK:ITEM Definition in Create Effect must be of type string");
                }
                string name = jproperty.Name;
                string str2 = "###";
                JToken jtoken3 = jobject[jproperty.Name];
                text = name + str2 + ((jtoken3 != null) ? jtoken3.ToString() : null);
            }
            //list.Add(text);
            GameObject gameObject;
            ModItemLoader.main.gameItemLookup.TryGetValue(text.ToLower(), out gameObject);
            if (gameObject == null)
            {
                ModLog.LogWarning(item.GetComponent<ModdedItem>().fromModpack.internalName, item.name, "Could not find " + text + ", adding Placeholder to be resolved after loading. This is not an Error. ");
                return ModItemLoader.main.CreatePlaceholder(text, item.gameObject).GetComponent<Item2>();
            }
            return gameObject.GetComponent<Item2>();
        }

        public static void ResolvePlaceholders(ref List<Item2> itemList)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                ModdedItem component = itemList[i].GetComponent<ModdedItem>();
                if (!(component != null) || component.placeholder == null || !(component.placeholder != ""))
                {
                    continue;
                }
                bool flag = false;
                Item2 value = null;
                if (component.placeholder.ToLower().StartsWith("bph###"))
                {
                    string text = component.placeholder.ToLower().Substring(3);
                    flag = ModItemLoader.main.modItemLookup.TryGetValue((component.fromModpack.internalName + text).ToLower(), out value);
                    foreach (ModLoader.ModpackInfo modpack in ModLoader.main.modpacks)
                    {
                        if (!flag)
                        {
                            flag = ModItemLoader.main.modItemLookup.TryGetValue((modpack.internalName + text).ToLower(), out value);
                        }
                    }
                }
                else
                {
                    flag = ModItemLoader.main.modItemLookup.TryGetValue(component.placeholder.ToLower(), out value);
                }
                if (!flag)
                {
                    throw new ModUtils.ParseException(component.thisObj.name + " will not work! " + component.placeholder.Split(new string[] { "###" }, StringSplitOptions.None)[1] + " is not a valid item");
                }
                ModLog.Log(component.fromModpack.internalName, component.thisObj.name, "Resolved " + component.thisObj.name + " placeholder: " + value.name);
                UnityEngine.Object.Destroy(itemList[i]);
                itemList[i] = value;
            }
        }

        public static void ResolvePlaceholder(ref Item2 item)
        {
            ModdedItem component = item.GetComponent<ModdedItem>();
            if (!(component != null) || component.placeholder == null || !(component.placeholder != ""))
            {
                return;
            }
            bool flag = false;
            Item2 value = null;
            if (component.placeholder.ToLower().StartsWith("bph###"))
            {
                string text = component.placeholder.ToLower().Substring(3);
                flag = ModItemLoader.main.modItemLookup.TryGetValue((component.fromModpack.internalName + text).ToLower(), out value);
                foreach (ModLoader.ModpackInfo modpack in ModLoader.main.modpacks)
                {
                    if (!flag)
                    {
                        flag = ModItemLoader.main.modItemLookup.TryGetValue((modpack.internalName + text).ToLower(), out value);
                    }
                }
            }
            else
            {
                flag = ModItemLoader.main.modItemLookup.TryGetValue(component.placeholder.ToLower(), out value);
            }
            if (!flag)
            {
                throw new ModUtils.ParseException(component.thisObj.name + " will not work! " + component.placeholder.Split(new string[] { "###" }, StringSplitOptions.None)[1] + " is not a valid item");
            }
            ModLog.Log(component.fromModpack.internalName, component.thisObj.name, "Resolved " + component.thisObj.name + " placeholder: " + value.name);
            UnityEngine.Object.Destroy(item);
            item = value;

        }
    }
}
