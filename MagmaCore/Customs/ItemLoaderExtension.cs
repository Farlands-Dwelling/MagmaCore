using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Customs
{
    public abstract class ItemLoaderExtension
    {
        public static List<ItemLoaderExtension> Extensions = new List<ItemLoaderExtension>();

        /// <summary>
        /// Ran everytime a modded item is being deserialized.
        /// </summary>
        /// <param name="itemGameObject">The item that is being deserialized's GameObject.</param>
        /// <param name="__instance">The instance of the ModItemLoader.</param>
        /// /// <param name="jobject">The deserialized JSON object of the item.</param>
        public virtual void Extension(ref GameObject itemGameObject, ref ModItemLoader __instance, JObject jobject) { }
    }
}
