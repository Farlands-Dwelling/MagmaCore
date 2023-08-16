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
    public abstract class CustomCharacter
    {
        public static Dictionary<int, CustomCharacter> CustomCharacters = new Dictionary<int, CustomCharacter>();
        public static Dictionary<KeyValuePair<string, string>, CustomCharacter> CustomCharactersByModName = new Dictionary<KeyValuePair<string, string>, CustomCharacter>();
        public static List<MonoBehaviour> CharacterManagers = new List<MonoBehaviour>();

        public string ModID = "";
        public string ModName = "";
        public int ID = 0;
        public Character CharacterInstance;

        public virtual string characterName { get; protected set; }
        public virtual string characterDescription { get; protected set; }
        public virtual string mapCharacterHoverText { get; protected set; }
        public virtual int startingHealth { get; protected set; } = 40;
        public virtual int defaultEnergyPerTurn { get; protected set; } = 3;

        public virtual Sprite portraitSprite { get; protected set; }
        //public virtual string characterNameKey { get; protected set; }
        //public virtual string characterDescriptionKey { get; protected set; }

        //public virtual List<GameObject> startingObjects { get; protected set; }
        public virtual List<ModItemDefinition> startingItems { get; protected set; }
        public virtual List<GameObject> startingObjectsForLimitedItemGet { get; protected set; }
        //public virtual List<RuntimeAnimatorController> animatorControllers { get; protected set; }
        public virtual List<Skin> skins { get; protected set; }
        public virtual List<float> characterSelectorSizeRatio { get; protected set; }
        public virtual List<float> yAdjustment { get; protected set; }

        public virtual Sprite standardGridSprite { get; protected set; }
        public virtual Sprite[] itemBorderBackgroundSprites { get; protected set; }
        public virtual ModularBackpack.BackpackPieces backpackPieces { get; protected set; }
        public virtual Vector3[] decalPositions { get; protected set; }
        public virtual Sprite mapSprite { get; protected set; }
        public virtual List<Sprite> mapCharacterSprite { get; protected set; }
        public virtual Sprite footstepSprite { get; protected set; }

        public virtual Vector2 defaultBagSize { get; protected set; }
        public virtual Vector2 endingBagSize { get; protected set; }
        public virtual Vector2 endingBagSizeDemo { get; protected set; }
        public virtual List<LevelUp> levelUps { get; protected set; }
        public virtual List<ActionButtonManager.Type> buttonTypes { get; protected set; }
        public virtual List<string> itemBlacklist { get; protected set; }
        public virtual List<string> itemWhitelist { get; protected set; }
        public virtual bool blacklistItemsAllowedForOneCharacter { get; protected set; } = true;
        public virtual List<string> itemWhitelistUsingCharacter { get; protected set; }

        public void Convert()
        {
            Character purse = Resources.FindObjectsOfTypeAll<Character>().ToList().Find(x => x.characterName == Character.CharacterName.Purse);
            Character result = ScriptableObject.Instantiate(purse);//ScriptableObject.CreateInstance<Character>();

            if (standardGridSprite != null) result.standardGridSprite = standardGridSprite;
            if (itemBorderBackgroundSprites != null) result.itemBorderBackgroundSprites = itemBorderBackgroundSprites;
            if (portraitSprite != null) result.portraitSprite = portraitSprite;
            if (characterName != null) result.characterName = (Character.CharacterName)GetHash();
            if (characterName != null) result.name = characterName;
            if (startingHealth != 0) result.startingHealth = startingHealth;
            if (defaultEnergyPerTurn != 0) result.defaultEnergyPerTurn = defaultEnergyPerTurn;
            if (startingObjectsForLimitedItemGet != null) result.startingObjectsForLimitedItemGet = startingObjectsForLimitedItemGet;
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

            CharacterInstance = result;

            if (skins != null) CreateAnimationOverrideController();
            CreateTranslations();
            CreateUIElements();
            CreateItemBlacklist();

            Modify(CharacterInstance);
        }

        public int GetHash()
        {
            return StringUtils.GetInt32HashCode($"{ModID}:{characterName}");
        }

        public static T RegisterCharacter<T>(T character) where T : CustomCharacter
        {
            if (character.ID == 0)
                character.ID = character.GetHash();

            if (CustomCharacters.ContainsKey(character.ID))
            {
                MelonLogger.Error($"Error while registering custom character \"{character.ModID}:{character.characterName}\". (Clashing with : {CustomCharacters[character.ID]})");
                return null;
            }

            CustomCharacters.Add(character.ID, character);
            CustomCharactersByModName.Add(new KeyValuePair<string, string>(character.ModName, character.characterName), character);

            MelonLogger.Msg($"{character.ModName},{character.characterName}");

            return character;
        }

        #region Create Methods
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

        private void CreateAnimationOverrideController()
        {
            CharacterInstance.animatorControllers.Clear();

            
            foreach (Skin animOverride in skins)
            {
                RuntimeAnimatorController playerController = Resources.FindObjectsOfTypeAll<RuntimeAnimatorController>().ToList().Find(x => x.name == "Player Controller");
                AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(playerController);
                animatorOverrideController.runtimeAnimatorController = playerController;

                IList<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

                for (int animOverrideIndex = 0; animOverrideIndex < animOverride.animationOverrides.Count; animOverrideIndex++)
                {
                    AnimatorOverridePair overridePair = animOverride.animationOverrides[animOverrideIndex];
                    for (int originalClipIndex = 0; originalClipIndex < animatorOverrideController.clips.Length; originalClipIndex++)
                    {
                        AnimationClip originalClip = animatorOverrideController.clips[originalClipIndex].originalClip;
                        if (originalClip.name == overridePair.originalClipName)
                        {
                            // Copy animation events to prevent "lag", for example when using an item or attacking an enemy
                            if (overridePair.overrideClip.length < originalClip.length && originalClip.events != null)
                                MelonLogger.Error($"[{ModName}] The length of clip '{overridePair.overrideClip.name}' ({overridePair.overrideClip.length}) is less than the length of the original clip `{originalClip.name}` ({originalClip.length}). This will cause the animation events of this animation to not load properly.");

                            overridePair.overrideClip.events = originalClip.events;

                            overrides.Add(
                                new KeyValuePair<AnimationClip, AnimationClip>(
                                    originalClip,
                                    overridePair.overrideClip));
                        }
                    }
                }

                animatorOverrideController.ApplyOverrides(overrides);
                CharacterInstance.animatorControllers.Add(animatorOverrideController);
            }
        }

        private void CreateTranslations()
        {
            string hoverTextKey = "map" + GetHash().ToString();
            if (mapCharacterHoverText != null)
            {
                TranslationUtils.CreateTranslation(hoverTextKey, mapCharacterHoverText);
            } else
            {
                TranslationUtils.CreateTranslation(hoverTextKey, Main.LangTerms["mappurse"]);
            }

            if (characterName != null)
            {
                CharacterInstance.characterNameKey = GetHash().ToString();
                TranslationUtils.CreateTranslation(CharacterInstance.characterNameKey, characterName);
            }
            if (characterDescription != null)
            { // concatenated "d" to differentiate from name key
                CharacterInstance.characterDescriptionKey = GetHash().ToString() + "d";
                TranslationUtils.CreateTranslation(CharacterInstance.characterDescriptionKey, characterDescription);
            }
        }

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

        public virtual void Modify(Character characterInstance) { }

        // Probably move these two out of this class.
        public struct Skin
        {
            public List<AnimatorOverridePair> animationOverrides;
        }

        public struct AnimatorOverridePair
        {
            public string originalClipName;
            public AnimationClip overrideClip;
        }

        private enum OriginalClips
        {
            Player_Attack,
            Player_Hurt,
            Player_Run,
            Player_Die,
            Player_UseItem,
            Player_ReadMap,
            Player_Win,
            Player_Block,
            Player_Attack_Overhead,
            Player_WalkAway,
            Player_FireArrow,
            Player_SearchPack,
            Player_Command
        }
    }
}
