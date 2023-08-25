using MagmaCore.Customs;
using MagmaCore.Utils;
using MelonLoader;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.ItemLoaderExtensions
{
    public class ModItemTypeExtension : ItemLoaderExtension
    {
        // "mod_itemtypes": [
        //      { "Farlands Realized": "Recipe" }
        //  ]
        public override void Extension(ref GameObject itemGameObject, ref ModItemLoader __instance, JObject jobject)
        {
            Item2 item = itemGameObject.GetComponent<Item2>();

            if (!ModUtils.IsNullEmpty(jobject["mod_item_types"]))
            {
                foreach (JToken jtoken in (IEnumerable<JToken>)(jobject["mod_item_types"]))
                {
                    if (jtoken.Type == JTokenType.Object)
                    {
                        JObject jobject2 = (JObject)jtoken;
                        if (!jobject2.HasValues)
                        {
                            return;
                        }

                        if (jobject2.Count > 1)
                        {
                            throw new ModUtils.SyntaxException("MODPACK:MOD ITEM TYPE Definitions cannot have more than one type inside object. Make seperate objects for each type inside the array.");
                        }

                        JProperty jproperty = (JProperty)((JObject)jtoken).First;
                        if (jobject2[jproperty.Name].Type != JTokenType.String)
                        {
                            throw new ModUtils.SyntaxException("MODPACK:MOD ITEM TYPE Definitions must be of type string");
                        }

                        string modName = jproperty.Name;
                        JToken jtoken3 = jobject2[jproperty.Name];
                        string typeName = ((jtoken3 != null) ? jtoken3.ToString() : null);
                        try
                        {
                            
                            if (!ItemUtils.CustomItemTypesByModName.ContainsKey(new KeyValuePair<string, string>(modName, typeName)))
                            {
                                Item2.ItemType itemType = ItemUtils.RegisterItemType(typeName, modName);
                                item.itemType.Add(itemType);
                            }
                            else
                            {
                                Item2.ItemType itemType = ItemUtils.CustomItemTypesByModName[new KeyValuePair<string, string>(modName, typeName)];
                                item.itemType.Add(itemType);
                            }
                        }
                        catch (Exception inner7)
                        {
                            throw new ModUtils.SyntaxException(typeName, inner7);
                        }
                    }
                }
            }
        }
    }
}
