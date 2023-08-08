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

namespace MagmaCore.Customs
{
    public abstract class CustomCharacter
    {
        public static List<CustomCharacter> CustomCharacters = new List<CustomCharacter>();

        public static List<Character> Characters = new List<Character>();

        public string ModID = "";
        public string ModName = "";
        public Character CharacterInstance;

        public virtual string characterName { get; protected set; }
        public virtual string characterDescription { get; protected set; }
        public virtual string mapCharacterHoverText { get; protected set; }
        public virtual int startingHealth { get; protected set; } = 40;
        public virtual int defaultEnergyPerTurn { get; protected set; } = 3;

        public virtual Sprite portraitSprite { get; protected set; }
        //public virtual string characterNameKey { get; protected set; }
        //public virtual string characterDescriptionKey { get; protected set; }

        public virtual List<GameObject> startingObjects { get; protected set; }
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
            if (startingObjects != null) result.startingObjects = startingObjects;
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

            Characters.Add(result);


            if (skins != null) CreateAnimationOverrideController();
            CreateTranslations();
            CreateUIElements();
        }

        public int GetHash()
        {
            return StringUtils.GetInt32HashCode($"{ModID}:{characterName}");
        }

        public static T RegisterCharacter<T>(T character) where T : CustomCharacter
        {
            /*if (GDOs.ContainsKey(character.ID))
            {
                Main.LogInfo($"Error while registering custom GDO of type {character.GetType().FullName} with ID={character.ID} and Name=\"{character.ModName}:{character.UniqueNameID}\". Double-check to ensure that the UniqueNameID is actually unique. (Clashing with : {GDOs[character.ID]})");
                return null;
            }*/

            CustomCharacters.Add(character);

            return character;
        }

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
            buttonCharacterIcon.sprite = mapCharacterSprite[0];

            Button button = iconButtonGameObject.GetComponent<Button>();

            object persistentCalls = HarmonyLib.AccessTools.Field(typeof(UnityEventBase), "m_PersistentCalls").GetValue(button.onClick);
            MethodInfo registerPersistentListener = HarmonyLib.AccessTools.Method(HarmonyLib.AccessTools.TypeByName("PersistentCallGroup"), "RegisterObjectPersistentListener", new Type[] { typeof(int), typeof(UnityEngine.Object), typeof(Type), typeof(UnityEngine.Object), typeof(string) });
            registerPersistentListener.Invoke(persistentCalls, new object[] { 0, characterSelection, typeof(NewCharacterSelector), CharacterInstance, "ChooseCharacter" });
            //button.onClick = new Button.ButtonClickedEvent();
            //button.onClick.AddListener(delegate { characterSelection.ChooseCharacter(CharacterInstance); }); 
        }

        private void CreateAnimationOverrideController()
        {
            /*IList<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (AnimatorOverridePair pair in animatorControllers)
            {
                AnimatorOverrideController animatorOverrideControllerTemplate = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<AnimatorOverrideController>().ToList().Find(x => x.name == "purse default"));
                CharacterInstance.animatorControllers.Clear();

                for (int i = 0; i < animatorOverrideControllerTemplate.clips.Length; i++)
                {
                    if (animatorOverrideControllerTemplate.clips[i].originalClip.name == pair.originalClipName)
                    {
                        overrides.Add(
                            new KeyValuePair<AnimationClip, AnimationClip>(
                                animatorOverrideControllerTemplate.clips[i].originalClip, 
                                pair.overrideClip));
                    }
                }

                animatorOverrideControllerTemplate.ApplyOverrides(overrides);
                CharacterInstance.animatorControllers.Add(animatorOverrideControllerTemplate);
            }*/
            CharacterInstance.animatorControllers.Clear();

            
            foreach (Skin animOverride in skins)
            {
                RuntimeAnimatorController playerController = Resources.FindObjectsOfTypeAll<RuntimeAnimatorController>().ToList().Find(x => x.name == "Player Controller");
                AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(playerController);
                animatorOverrideController.runtimeAnimatorController = playerController;

                IList<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

                foreach (AnimatorOverridePair pair in animOverride.animationOverrides)
                {
                    for (int i = 0; i < animatorOverrideController.clips.Length; i++)
                    {
                        if (animatorOverrideController.clips[i].originalClip.name == pair.originalClipName)
                        {
                            // Copy animation events to prevent "lag", for example when using an item or attacking an enemy
                            pair.overrideClip.events = animatorOverrideController.clips[i].originalClip.events;

                            overrides.Add(
                                new KeyValuePair<AnimationClip, AnimationClip>(
                                    animatorOverrideController.clips[i].originalClip,
                                    pair.overrideClip));
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
