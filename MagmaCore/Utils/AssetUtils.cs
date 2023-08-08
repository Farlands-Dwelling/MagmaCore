using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Utils
{
    public static class AssetUtils
    {
        public static AssetBundle QuickLoadAssetBundle(string assetBundleName)
        {
            string AssetBundlePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), assetBundleName);

            return AssetBundle.LoadFromFile(AssetBundlePath);
        }

        public static AssetBundle QuickLoadAssetBundle(string assetBundleName, string assetBundleLocation)
        {
            string AssetBundlePath = Path.Combine(Path.GetDirectoryName(assetBundleLocation), assetBundleName);

            return AssetBundle.LoadFromFile(AssetBundlePath);
        }
    }
}
