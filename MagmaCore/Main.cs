using MagmaCore.Customs;
using MagmaCore.ItemLoaderExtensions;
using MagmaCore.Managers;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore
{
    public class Main : MagmaMod
    {
        public override string InternalModName => "MagmaCore";

        public static readonly List<Character> Characters = new List<Character>();
        public static List<MonoBehaviour> Managers = new List<MonoBehaviour>();
        public static readonly int ExtraItemFunctionNum = 555; //TODO: placeholder, should definitely find a better way than using numbers in jsons

        public override void OnInitializeMagma()
        {
            AddItemLoaderExtension<ModItemTypeExtension>();

            AddManager<MagmaManager>();
        }

        public override void OnFirstMainMenuLoad()
        {
            foreach (CustomBase custom in CustomBase.Customs.Values)
            {
                custom.Convert();
            }
        }

        public override void OnPostSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Game")
            {
                foreach (MonoBehaviour manager in Managers)
                {
                    GameObject.FindObjectOfType<GameManager>().gameObject.AddComponent(manager.GetType());
                }
            }
        }
    }
}
