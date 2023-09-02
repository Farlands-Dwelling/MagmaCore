using MagmaCore.Customs;
using MagmaCore.Patches;
using MagmaCore.Utils;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace MagmaCore
{
    public abstract class MagmaMod : MelonMod
    {
        public abstract string InternalModName { get; }
        public virtual List<string> Dependencies { get; private set; } = new List<string>();

        public static Dictionary<string, string> LangTerms = new Dictionary<string, string>();
        public static LangaugeManager LangManager;

        public static string LoadedScene;

        public bool FirstLoad = true;


        public sealed override void OnInitializeMelon()
        {
            //ModsFinishedLoadingPatch.OnLoadedMods += OnLoadedMods;
            //ModsFinishedLoadingPatch.OnLoadedMods += HandleDependencies;
            MelonCoroutines.Start(WaitUntilModsLoaded());

            OnInitializeMagma();
        }

        IEnumerator WaitUntilModsLoaded()
        {
            yield return new WaitUntil(() => GameObject.FindObjectOfType<ModLoader>() != null);
            yield return new WaitUntil(() => ModLoader.main.dataReady);
            OnLoadedMods();
        }

        public sealed override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            LoadedScene = sceneName;

            if (sceneName == "MainMenu")
            {
                if (FirstLoad)
                {
                    SetFields();
                    OnFirstMainMenuLoad();
                    FirstLoad = false;
                }
            }

            OnPostSceneWasLoaded(buildIndex, sceneName);
        }

        private void SetFields()
        {
            LangManager = GameObject.FindObjectOfType<LangaugeManager>();
            LangTerms = LangManager.languageTerms;
        }

        public virtual void OnInitializeMagma() { }

        public virtual void OnLoadedMods() { }

        public virtual void OnFirstMainMenuLoad() { }

        public virtual void OnPostSceneWasLoaded(int buildIndex, string sceneName) { }

        public T AddCustom<T>() where T : CustomBase, new()
        {
            T custom = new T();
            custom.ModID = InternalModName;
            custom.ModName = Info.Name;

            return CustomBase.RegisterCustom(custom);
        }

        /// <summary>
        /// Adds the specified class as a component on the GameManager during a run.
        /// </summary>
        /// <typeparamref name="T">The component to add to the GameManager.</typeparamref>
        /// <returns>Instance of newly created component</returns>
        public T AddManager<T>() where T : MonoBehaviour, new()
        {
            T component = new T();
            Main.Managers.Add(component);
            return component;
        }

        public T AddItemLoaderExtension<T>() where T : ItemLoaderExtension, new()
        {
            T extension = new T();
            ItemLoaderExtension.Extensions.Add(extension);
            return extension;
        }

        // When main menu is opened, instantiate a popup that says what mods you need
        // For now it will say dependency missing, but when the ModLoader is added to the Main Menu scene, change it so it checks when main menu is loaded (and item mods are fully initalized)
        protected void HandleDependencies()
        {
            List<string> missingDependencies = new List<string>();
            foreach (string modName in Dependencies)
            {
                if (ModpackUtils.GetModpackFromInternalName(modName) == null)
                {
                    missingDependencies.Add(modName);
                }
            }
        }
    }
}
