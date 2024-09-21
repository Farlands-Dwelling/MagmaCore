using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventButton;
using static MelonLoader.MelonLogger;
using UnityEngine;
using MagmaCore.Utils;
using static Character;

namespace MagmaCore.Customs
{
    public abstract class CustomRandomEventMaster : CustomBase
    {
        public RandomEventMaster Instance;
        public virtual List<EventButton> eventButtons { get; private set; }
        // TODO: maybe add character specific flavor text. aka eventTextKey + "n" + Player.main.characterName.ToString();
        public virtual string eventName { get; private set; } = "No name set.";
        public virtual string flavorText { get; private set; } = "No flavor text set.";
        public override bool ConvertOnGameLoad => true;

        public override void Convert()
        {
            RandomEventMaster eventMaster = Resources.FindObjectsOfTypeAll<EventNPC>().ToList().Find(x => x.name == "Healer NPC Variant").eventPrefab.GetComponent<RandomEventMaster>();
            RandomEventMaster result = GameObject.Instantiate(eventMaster.gameObject).GetComponent<RandomEventMaster>();
            Instance = result;
            Instance.name = UniqueNameID;

            foreach (Transform child in Instance.buttons.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (EventButton eventButton in eventButtons)
            {
                GameObject buttonGO = GameObject.Instantiate(eventButton.gameObject);
                buttonGO.transform.SetParent(Instance.buttons.transform);
                //TranslationUtils.CreateTranslation("english", Instance.eventTextKey + "b" + i, eventButton.buttonText);
            }

            Instance.transform.SetParent(Main.Hider.transform);

            CreateTranslations();
        }

        private void CreateTranslations()
        {
            Instance.eventTextKey = GetHash().ToString();
            TranslationUtils.CreateTranslation("english", Instance.eventTextKey + "n", eventName);
            TranslationUtils.CreateTranslation("english", Instance.eventTextKey + "o1", flavorText);
        }
    }
}
