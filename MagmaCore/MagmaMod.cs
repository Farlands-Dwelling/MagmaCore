using MagmaCore.Customs;
using MagmaCore.Patches;
using MagmaCore.Utils;
using MelonLoader;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace MagmaCore
{
    public abstract class MagmaMod : MelonMod
    {
        public abstract string InternalModName { get; }
        /// <value>
        /// List of workshop IDs for mods that should be installed on game launched. You can find the ID of a workshop item by right clicking on the workshop page, copying the page URL, and looking at ID path in the URL.
        /// </value>
        public virtual List<ulong> Dependencies { get; private set; } = new List<ulong>();
        private List<ulong> PreInstalledMods = new List<ulong>();
        private List<ModLoader.ModpackInfo> EnabledDependencies = new List<ModLoader.ModpackInfo>();

        public static Dictionary<string, string> LangTerms = new Dictionary<string, string>();
        public static LangaugeManager LangManager;

        public static string LoadedScene;

        public bool FirstLoad = true;


        public sealed override void OnInitializeMelon()
        {
            MelonCoroutines.Start(WaitUntilModsLoaded());

            OnInitializeMagma();
        }

        public sealed override void OnApplicationStart()
        {
            MelonCoroutines.Start(SubscribeToDependencies());
            Callback<Steamworks.ItemInstalled_t>.Create(EnableDependency);
        }

        public sealed override void OnApplicationQuit()
        {
            UnsubscribeFromDependencies();
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

        protected virtual void OnEnabledAllDependencies() { }

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

        private IEnumerator SubscribeToDependencies()
        {
            yield return new WaitUntil(() => SteamManager.s_EverInitialized == true);

            // Create a list of already subscribed mods, so that when it uninstalls mods, it won't uninstall a wanted mod
            if (SteamManager.s_EverInitialized)
            {
                PublishedFileId_t[] fileIDs = new PublishedFileId_t[SteamUGC.GetNumSubscribedItems()];
                SteamUGC.GetSubscribedItems(fileIDs, SteamUGC.GetNumSubscribedItems());
                foreach (var ID in fileIDs)
                {
                    PreInstalledMods.Add(ID.m_PublishedFileId);
                }

                foreach (var ID in Dependencies)
                {
                    if (!PreInstalledMods.Contains(ID))
                        SteamUGC.SubscribeItem(new PublishedFileId_t(ID));
                }
            }
        }
        private void UnsubscribeFromDependencies()
        {
            foreach (var ID in Dependencies)
            {
                if (!PreInstalledMods.Contains(ID))
                    SteamUGC.UnsubscribeItem(new PublishedFileId_t(ID));
            }
        }

        private void EnableDependency(Steamworks.ItemInstalled_t pCallback)
        {
            ulong fileId = pCallback.m_nPublishedFileId.m_PublishedFileId;

            MelonCoroutines.Start(EnableMod(fileId));
        }

        private IEnumerator EnableMod(ulong fileId)
        {
            yield return new WaitUntil(() => GameObject.FindObjectOfType<ModLoader>() != null);
            yield return new WaitUntil(() => ModLoader.main.dataReady);

            ModLoader.main.ReloadModpacks();
            ModMetaSave.SaveModData();

            if (Dependencies.Contains(fileId))
            {
                ModLoader.ModpackInfo mod = ModLoader.main.modpacks.FindAll(x => x.workshop != null).Find(x => x.workshop.fileId.m_PublishedFileId == fileId);
                if (mod != null)
                {
                    if (!mod.loaded)
                    {
                        ModLoader.main.LoadModpack(mod);
                    }
                    yield return new WaitUntil(() => mod.loaded);
                    EnabledDependencies.Add(mod);

                    if (EnabledDependencies.Count >= Dependencies.Count)
                    {
                        OnEnabledAllDependencies();
                        yield break;
                    }
                }
            }
        }
    }
}
