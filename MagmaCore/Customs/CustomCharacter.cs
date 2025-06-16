using Harmony;
using MagmaCore.Datatypes;
using MagmaCore.Utils;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static Character;
using static Item2;
using static MagmaCore.Customs.CustomCharacter;
using static MelonLoader.Modules.MelonModule;

namespace MagmaCore.Customs
{
    public abstract class CustomCharacter : CustomBase
    {
        /// <summary>
        /// Wrapper class for the <c>Character</c> class.
        /// </summary>
        public Character CharacterInstance;

        /// <value>
        /// Represents the name of the character in the game. This property is used to set and retrieve a character's name and is protected to only allow modification within the class or by derived classes.
        /// </value>
        public virtual string characterName { get; protected set; }

        /// <value>
        /// The description that shows up for the character in Character Select.
        /// </value>
        public virtual string characterDescription { get; protected set; }

        /// <value>
        /// The text displayed when hovering over the character on the map.
        /// Usually a short sentence from the perspective of the character.
        /// </value>
        public virtual string mapCharacterHoverText { get; protected set; }

        /// <value>
        /// Represents the initial health value for a custom character.
        /// </value>
        public virtual int startingHealth { get; protected set; } = 40;

        /// <summary>
        /// Represents the default energy allocated to a character per turn.
        /// </summary>
        public virtual int defaultEnergyPerTurn { get; protected set; } = 3;

        /// <value>
        /// Displayed in Character Select when you select this character.
        /// </value>
        public virtual Sprite portraitSprite { get; protected set; }
        //public virtual string characterNameKey { get; protected set; }
        //public virtual string characterDescriptionKey { get; protected set; }

        //public virtual List<GameObject> startingObjects { get; protected set; }
        /// <value>
        /// A list of items that are available at the start for a character. These items are defined by the ModItemDefinition structure and can include any game objects relevant to the character's initial setup.
        /// </value>
        public virtual List<ModItemDefinition> startingItems { get; protected set; }

        /// <value>
        /// A list of GameObjects representing the initial objects available for characters with limited item access. This property can be set and protected modifications can be applied in derived classes.
        /// </value>
        public virtual List<GameObject> startingObjectsForLimitedItemGet { get; protected set; }
        //public virtual List<RuntimeAnimatorController> animatorControllers { get; protected set; }
        /// <value>
        /// A collection of runtime animator controllers used to define different skins for the character.
        /// These controllers are typically animation override controllers. To facilitate the process, use CustomSkins and the SkinInstance property.
        /// </value>
        public virtual List<RuntimeAnimatorController> skins { get; protected set; }

        /// <value>
        /// A list of floating-point values that represent the size ratio for character selectors.
        /// This property allows customization of the character display scaling within a selector interface.
        /// </value>
        public virtual List<float> characterSelectorSizeRatio { get; protected set; }

        /// <value>
        /// A list of adjustments applied to the vertical positioning of character elements.
        /// These float values determine the offset on the Y-axis for rendering or gameplay purposes.
        /// </value>
        public virtual List<float> yAdjustment { get; protected set; }

        /// <value>
        /// Represents the sprite used as the default visual representation for the character in a standard grid layout.
        /// </value>
        public virtual Sprite standardGridSprite { get; protected set; }

        /// <value>
        /// An array of sprites used to define the background for item borders in a character.
        /// </value>
        public virtual Sprite[] itemBorderBackgroundSprites { get; protected set; }

        /// <value>
        /// Represents the components or pieces of the modular backpack associated with the character.
        /// This property is used to manage and define different sections or elements of the backpack,
        /// allowing for modular customizations and configurations specific to the character.
        /// </value>
        public virtual ModularBackpack.BackpackPieces backpackPieces { get; protected set; }

        /// <value>
        /// An array of Vector3 coordinates representing the positions where decals should be placed on the character model.
        /// </value>
        public virtual Vector3[] decalPositions { get; protected set; }

        /// <value>
        /// Represents the sprite used as the visual representation of the character on the map.
        /// </value>
        public virtual Sprite mapSprite { get; protected set; }

        /// <value>
        /// A list of sprites representing the character's appearance on the map. This can include different visual states or expressions of the character as they appear in the map environment.
        /// </value>
        public virtual List<Sprite> mapCharacterSprite { get; protected set; }

        /// <value>
        /// A sprite used to represent the character's footprint or footstep in the game.
        /// </value>
        public virtual Sprite footstepSprite { get; protected set; }

        /// <value>
        /// Represents the default size of the character's bag in vector dimensions, determining the initial capacity for carrying items.
        /// </value>
        public virtual Vector2 defaultBagSize { get; protected set; }

        /// <value>
        /// Specifies the size of the character's bag at the end of a game or scenario.
        /// It determines the inventory capacity the character has in their final state.
        /// </value>
        public virtual Vector2 endingBagSize { get; protected set; }

        /// <value>
        /// Represents the bag size to be used for demonstration purposes at the end of a particular scenario or process.
        /// This property is used to define a specific size of a character's bag when illustrating or testing the conclusion
        /// of an event or condition. It is intended to simulate different bag configurations in a demo setting.
        /// </value>
        public virtual Vector2 endingBagSizeDemo { get; protected set; }

        /// <value>
        /// A list of level-up configurations applicable to the character. These configurations define the game mechanics and attributes that change as the character levels up.
        /// </value>
        public virtual List<LevelUp> levelUps { get; protected set; }

        /// <value>
        /// The list of action button types associated with the custom character. This list determines the types of actions the character can perform using buttons.
        /// </value>
        public virtual List<ActionButtonManager.Type> buttonTypes { get; protected set; }

        /// <value>
        /// A list of item names that determine which items can not spawn while playing this character.
        /// </value>
        public virtual List<string> itemBlacklist { get; protected set; }

        /// <value>
        /// A list of item names that determine which items are allowed to spawn while playing this character.
        /// Has priority over blacklist.
        /// </value>
        public virtual List<string> itemWhitelist { get; protected set; }

        /// <value>
        /// When set to true, character-specific items such as Cleavers or Components are automatically blacklisted for this character.
        /// </value>
        public virtual bool blacklistItemsAllowedForOneCharacter { get; protected set; } = true;

        /// <value>
        /// A list of character names used to determine if certain items should be whitelisted for specific characters.
        /// When populated, items associated with these character names will be considered for whitelisting regardless
        /// of their general blacklist status.
        /// </value>
        public virtual List<string> itemWhitelistUsingCharacter { get; protected set; }

        /// Converts the current object to a Character instance by copying various character-related properties
        /// from the current instance to a newly instantiated Character object. This method searches for the
        /// Character object with the character name 'Purse', clones it, and sets its properties based on the
        /// current instance's attributes.
        /// Upon conversion, additional initialization steps are invoked such as creating translations,
        /// UI elements, item blacklists, and potentially modifying the Character instance.
        /// The process involves checking if each attribute from the current object is not null or has a non-default
        /// value before assigning it to the corresponding attribute on the Character instance.
        public override void Convert()
        {
            Character purse = Resources.FindObjectsOfTypeAll<Character>().ToList().Find(x => x.characterName == Character.CharacterName.Purse);
            Character result = ScriptableObject.Instantiate(purse);//ScriptableObject.CreateInstance<Character>();
            CharacterInstance = result;

            if (standardGridSprite != null) result.standardGridSprite = standardGridSprite;
            if (itemBorderBackgroundSprites != null) result.itemBorderBackgroundSprites = itemBorderBackgroundSprites;
            if (portraitSprite != null) result.portraitSprite = portraitSprite;
            if (characterName != null) result.characterName = (Character.CharacterName)GetHash();
            if (characterName != null) result.name = characterName;
            if (startingHealth != 0) result.startingHealth = startingHealth;
            if (defaultEnergyPerTurn != 0) result.defaultEnergyPerTurn = defaultEnergyPerTurn;
            if (startingObjectsForLimitedItemGet != null) result.startingObjectsForLimitedItemGet = startingObjectsForLimitedItemGet;
            if (skins != null) result.animatorControllers = skins;
            if (characterSelectorSizeRatio != null) result.characterSelectorSizeRatio = characterSelectorSizeRatio;
            if (yAdjustment != null) result.yAdjustment = yAdjustment;
            if (backpackPieces != null) result.backpackPieces = backpackPieces;
            if (decalPositions != null) result.decalPositions = decalPositions;
            if (mapSprite != null) result.mapSprite = mapSprite;
            if (mapCharacterSprite != null) result.mapCharacterSprite = mapCharacterSprite;
            if (footstepSprite != null) result.footstepSprite = footstepSprite;
            if (defaultBagSize != Vector2.zero) result.defaultBagSize = defaultBagSize;
            if (endingBagSize != Vector2.zero) result.endingBagSize = endingBagSize;
            if (endingBagSizeDemo != Vector2.zero) result.endingBagSizeDemo = endingBagSizeDemo;
            if (levelUps != null) result.levelUps = levelUps;
            if (buttonTypes != null) result.buttonTypes = buttonTypes;

            /*result.standardGridSprite = standardGridSprite;
            result.itemBorderBackgroundSprites = itemBorderBackgroundSprites;
            result.portraitSprite = portraitSprite;
            result.characterNameKey = characterNameKey;
            result.characterDescriptionKey = characterDescriptionKey;
            result.characterName = (Character.CharacterName)GetHash();
            result.name = characterName;
            result.startingHealth = startingHealth;
            result.defaultEnergyPerTurn = defaultEnergyPerTurn;
            result.startingObjects = startingObjects;
            result.startingObjectsForLimitedItemGet = startingObjectsForLimitedItemGet;
            result.animatorControllers = animatorControllers;
            result.characterSelectorSizeRatio = characterSelectorSizeRatio;
            result.yAdjustment = yAdjustment;
            result.backpackPieces = backpackPieces;
            result.decalPositions = decalPositions;
            result.mapSprite = mapSprite;
            result.mapCharacterSprite = mapCharacterSprite;
            result.footstepSprite = footstepSprite;
            result.defaultBagSize = defaultBagSize;
            result.endingBagSize = endingBagSize;
            result.endingBagSizeDemo = endingBagSizeDemo;
            result.levelUps = levelUps;
            result.buttonTypes = buttonTypes;*/


            CreateTranslations();
            CreateUIElements();
            CreateItemBlacklist();

            Modify(ref CharacterInstance);
        }

        #region Create Methods

        /// <summary>
        /// Creates and initializes a set of UI elements for character selection.
        /// </summary>
        /// <remarks>
        /// This method locates the character selector UI component within the game resources.
        /// It creates a new button element based on a template found within the character selector,
        /// setting its parent and appearance. The button's associated icon image is configured using
        /// a predefined sprite if available. Additionally, the button's click event is dynamically
        /// linked to a character selection method using reflection to register a persistent listener.
        /// </remarks>
        private void CreateUIElements()
        {
            NewCharacterSelector characterSelection = Resources.FindObjectsOfTypeAll<NewCharacterSelector>()[0];

            if (characterSelection == null)
            {
                MelonLogger.Msg("The character selector is null!");
                return;
            }
            Transform iconButtonGameObjectTemplate = characterSelection.transform.Find("Character Select Master/Character Select/Character Selection List/Character Icon");
            GameObject iconButtonGameObject = GameObject.Instantiate(iconButtonGameObjectTemplate.gameObject);
            iconButtonGameObject.transform.parent = characterSelection.transform.Find("Character Select Master/Character Select/Character Selection List");

            Image buttonCharacterIcon = iconButtonGameObject.transform.Find("GameObject").GetComponent<Image>();

            if (mapCharacterSprite != null) buttonCharacterIcon.sprite = mapCharacterSprite[0];

            Button button = iconButtonGameObject.GetComponent<Button>();

            object persistentCalls = HarmonyLib.AccessTools.Field(typeof(UnityEventBase), "m_PersistentCalls").GetValue(button.onClick);
            MethodInfo registerPersistentListener = HarmonyLib.AccessTools.Method(HarmonyLib.AccessTools.TypeByName("PersistentCallGroup"), "RegisterObjectPersistentListener", new Type[] { typeof(int), typeof(UnityEngine.Object), typeof(Type), typeof(UnityEngine.Object), typeof(string) });
            registerPersistentListener.Invoke(persistentCalls, new object[] { 0, characterSelection, typeof(NewCharacterSelector), CharacterInstance, "ChooseCharacter" });
        }

        /// <summary>
        /// Generates translation keys and creates translations for a custom character.
        /// This method constructs translation keys based on the hash of the character
        /// and uses them to store hover text, character name, and description in a
        /// specified language using a translation utility. If the hover text is not
        /// provided, a default message is used.
        /// </summary>
        private void CreateTranslations()
        {
            string hoverTextKey = "map" + GetHash().ToString();
            if (mapCharacterHoverText != null)
            {
                TranslationUtils.CreateTranslation("english", hoverTextKey, mapCharacterHoverText);
            } else
            {
                TranslationUtils.CreateTranslation("english", hoverTextKey, "No description set.");
            }

            if (characterName != null)
            {
                CharacterInstance.characterNameKey = GetHash().ToString();
                TranslationUtils.CreateTranslation("english", CharacterInstance.characterNameKey, characterName);
            }
            if (characterDescription != null)
            { // concatenated "d" to differentiate from name key
                CharacterInstance.characterDescriptionKey = GetHash().ToString() + "d";
                TranslationUtils.CreateTranslation("english", CharacterInstance.characterDescriptionKey, characterDescription);
            }
        }

        /// <summary>
        /// Updates the item blacklist for a custom character. This involves filtering the items based
        /// on specific conditions and updating their availability for the character. It works with
        /// various lists such as character names, item whitelist, and item blacklist to determine
        /// which items should be associated with the character.
        /// </summary>
        /// <remarks>
        /// The method processes all available items and performs checks to determine if an item should
        /// be blacklisted or available based on specific conditions. This includes checks against a
        /// whitelist and blacklist of items, and considerations for items valid for specific characters.
        /// </remarks>
        private void CreateItemBlacklist()
        {
            // TODO: Make use ItemUtils
            // TODO: Refactor + add item pool list maybe? + make it happen after modded items load

            Item2[] items = Resources.FindObjectsOfTypeAll<Item2>();

            for (int i = 0; i < items.Length; i++)
            {
                Item2 item = items[i];
                List<string> characterNames = new List<string>();
                bool hasSoloBlacklistedCharacter = false;
                bool itemIsBlacklisted = false;

                foreach (CharacterName name in item.validForCharacters)
                {
                    characterNames.Add(name.ToString());
                }

                if (blacklistItemsAllowedForOneCharacter == true)
                    hasSoloBlacklistedCharacter = item.validForCharacters.Count <= 1;
                if (itemWhitelistUsingCharacter != null)
                {
                    if (characterNames.Intersect(itemWhitelistUsingCharacter).Any())
                    {
                        hasSoloBlacklistedCharacter = false;
                    }
                }
                if (itemBlacklist != null)
                {
                    if (itemBlacklist.Contains(item.displayName))
                    {
                        itemIsBlacklisted = true;
                    }
                }
                if (itemWhitelist != null)
                {
                    if (itemWhitelist.Contains(item.displayName))
                    {
                        itemIsBlacklisted = false;
                    }
                }

                if (itemIsBlacklisted || hasSoloBlacklistedCharacter)
                    continue;

                item.validForCharacters.Add(CharacterInstance.characterName);
            }
        }

        #endregion

        /// <summary>
        /// Allows for modification of a character instance generated during the conversion process.
        /// This method is intended to be overridden by subclasses to implement custom modifications
        /// to the character instance based on specific game logic or requirements.
        /// </summary>
        /// <param name="characterInstance">A reference to the character instance that can be modified.</param>
        public virtual void Modify(ref Character characterInstance) { }
    }
}
