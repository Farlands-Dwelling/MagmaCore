using MagmaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaCore.Customs
{
    public abstract class CustomEventButton : CustomBase
    {
        public EventButton Instance;
        public virtual string buttonText { get; private set; }
        public virtual List<EventButton.PossibleOutcome> possibleOutcomes { get; private set; }
        public virtual bool skippable { get; private set; }
        public virtual GameObject requiredItem { get; private set; }
        public virtual List<Item2.Rarity> requiredRarities { get; private set; }
        public virtual List<Item2.ItemType> requiredItemType { get; private set; }
        public virtual int requiredGold { get; private set; }
        public virtual EventButton.Requirements requirement { get; private set; } = EventButton.Requirements.none;
        public virtual List<Character.CharacterName> validForCharacters { get; private set; }
        public virtual string overrideButtonTextKey { get; private set; }
        public virtual bool onlyGiveValidItems { get; private set; }
        public virtual GameObject specialItemToSpawn { get; private set; }
        public virtual RandomEventMaster randomEventMaster { get; private set; }
        public override bool ConvertOnGameLoad => true;

        public override void Convert()
        {
            EventButton eventButton = Resources.FindObjectsOfTypeAll<EventButton>()[0];
            Instance = GameObject.Instantiate(eventButton.gameObject).GetComponent<EventButton>();
            Instance.name = UniqueNameID;
            Instance.buttonText = buttonText;
            if (possibleOutcomes != null)
                Instance.possibleOutcomes = possibleOutcomes.ToArray();
            Instance.skippable = skippable;
            Instance.requiredItem = requiredItem;
            Instance.requiredRarities = requiredRarities;
            Instance.requiredItemType = requiredItemType;
            Instance.requiredGold = requiredGold;
            Instance.requirement = requirement;
            Instance.validForCharacters = validForCharacters;
            Instance.overrideButtonTextKey = overrideButtonTextKey;
            Instance.onlyGiveValidItems = onlyGiveValidItems;
            Instance.specialItemToSpawn = specialItemToSpawn;

            Instance.overrideButtonTextKey = TranslationUtils.GetOrCreateTranslation("english", GetHash().ToString(), buttonText).Key;

            if (randomEventMaster != null)
            {
                Instance.randomEventMaster = randomEventMaster;
            }

            Instance.transform.SetParent(Main.Hider.transform);
        }
        public virtual void OnClick()
        {/*
            foreach (EventButton.EventButtonAction eventButtonAction in Instance.chosenOutCome.eventButtonActions)
            {
                if (eventButtonAction.action == (EventButton.EventButtonAction.Action)StringUtils.GetInt32HashCode("Lalalala"))
                {
                }
            }
        */
        }
    }
}
