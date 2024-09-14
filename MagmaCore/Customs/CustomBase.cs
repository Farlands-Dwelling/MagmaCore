using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Character;
using static ModularBackpack;
using UnityEngine;
using UnityEngine.Playables;
using MagmaCore.Utils;
using MelonLoader;
using UnityEngine.TextCore.Text;

namespace MagmaCore.Customs
{
    public abstract class CustomBase
    {
        public static Dictionary<int, CustomBase> Customs = new Dictionary<int, CustomBase>();
        public static Dictionary<KeyValuePair<string, string>, CustomBase> CustomsByModName = new Dictionary<KeyValuePair<string, string>, CustomBase>();
        public static Dictionary<KeyValuePair<string, string>, CustomBase> CustomsByGUID = new Dictionary<KeyValuePair<string, string>, CustomBase>();
        public static Dictionary<Type, CustomBase> CustomsByType = new Dictionary<Type, CustomBase>();

        public Type ConvertedCustom;

        public virtual int ID { get; internal set; }
        public abstract string UniqueNameID { get; }
        public virtual bool UniqueConversionMethod { get; internal set; } = false;

        public string ModID = "";
        public string ModName = "";

        public abstract void Convert();

        public static T RegisterCustom<T>(T custom) where T : CustomBase
        {
            if (custom.ID == 0)
                custom.ID = custom.GetHash();

            if (Customs.ContainsKey(custom.ID))
            {
                MelonLogger.Error($"Error while registering custom \"{custom.ModID}:{custom.UniqueNameID}\". (Clashing with : {Customs[custom.ID]})");
                return null;
            }

            Customs.Add(custom.ID, custom);
            CustomsByType.Add(custom.GetType(), custom);
            CustomsByGUID.Add(new KeyValuePair<string, string>(custom.ModID, custom.UniqueNameID), custom);
            CustomsByModName.Add(new KeyValuePair<string, string>(custom.ModName, custom.UniqueNameID), custom);
            MelonLogger.Msg($"{custom.ModName},{custom.UniqueNameID}");
            return custom;
        }

        public int GetHash()
        {
            return StringUtils.GetInt32HashCode($"{ModID}:{UniqueNameID}");
        }
    }
}
