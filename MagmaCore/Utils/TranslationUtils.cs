using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaCore.Utils
{
    public static class TranslationUtils
    {
        //add language eventually
        public static void CreateTranslation(string key, string value)
        {
            MelonLogger.Msg(value);
            if (!Main.LangTerms.ContainsKey(key))
            {
                Main.LangTerms.Add(key, value);
            }
        }
    }
}
