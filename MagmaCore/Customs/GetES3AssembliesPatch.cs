using ES3Internal;
using HarmonyLib;
using MagmaCore.Utils;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagmaCore.Customs
{
    [HarmonyPatch(typeof(ES3Reflection), nameof(ES3Reflection.Assemblies), MethodType.Getter)]
    class GetES3AssembliesPatch
    {
        static void Postfix(ref Assembly[] __result)
        {
            List<Assembly> assemblies = new List<Assembly>(__result);

            foreach (Assembly a in AssetUtils.GetModdedAssemblies())
            {
                MelonLogger.Msg(a.FullName);
                if (!__result.ToList().Contains(a))
                    assemblies.Add(a);
            }
            __result = assemblies.ToArray();
        }
    }
}
