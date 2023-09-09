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
        /// <summary>
        /// Adds a translation to the game using the given language.
        /// </summary>
        /// <param name="language">The language this translation will be used for. The fallback language for this translation will be whichever language the it is first created with.</param>
        /// <param name="key">The key that will be used to find the translation value.</param>
        /// <param name="value">The text of the translation in the given language.</param>
        public static void CreateTranslation(string language, string key, string value)
        {
            key = key.ToLower().Trim();
            language = language.ToLower().Trim();

            if (!ModLoader.main.languageTerms.ContainsKey(language))
            {
                ModLoader.main.languageTerms.Add(language, new Dictionary<string, string>());
                MelonLogger.Error($"Language not found: {language}. Language has been created.");
            }
                

            if (!ModLoader.main.languageTerms[language].ContainsKey(key))
            {
                ModLoader.main.languageTerms[language].Add(key, value);
            }

            foreach (KeyValuePair<string, Dictionary<string, string>> langTerm in ModLoader.main.languageTerms)
            {
                if (!langTerm.Value.ContainsKey(key))
                {
                    ModLoader.main.languageTerms[langTerm.Key].Add(key, value);
                }
            }
        }

        public static KeyValuePair<string, string> GetOrCreateTranslation(string language, string key, string value)
        {
            CreateTranslation(language, key, value);

            return new KeyValuePair<string, string>(key, value);
        }
    }
}
