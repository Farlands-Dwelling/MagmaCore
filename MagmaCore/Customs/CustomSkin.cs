using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Customs
{
    public abstract class CustomSkin : CustomBase
    {
        public AnimatorOverrideController SkinInstance;

        public virtual Skin skin { get; private set; }
        public virtual List<Character> characters { get; private set; }

        public override void Convert()
        {
            SkinInstance = CreateAnimationOverrideController();
            if (characters != null)
            {
                if (characters.Count != 0) 
                {
                    foreach (Character character in characters)
                    {
                        character.animatorControllers.Add(SkinInstance);
                    }
                }
            }

            Modify(ref SkinInstance);
        }

        public virtual void Modify(ref AnimatorOverrideController skinInstance) { }

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
            Attack,
            Hurt,
            Run,
            Die,
            UseItem,
            ReadMap,
            Win,
            Block,
            AttackOverhead,
            WalkAway,
            FireArrow,
            SearchPack,
            Command
        }

        private AnimatorOverrideController CreateAnimationOverrideController()
        {
            RuntimeAnimatorController playerController = Resources.FindObjectsOfTypeAll<RuntimeAnimatorController>().ToList().Find(x => x.name == "Player Controller");
            AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(playerController);
            animatorOverrideController.runtimeAnimatorController = playerController;

            IList<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            for (int animOverrideIndex = 0; animOverrideIndex < skin.animationOverrides.Count; animOverrideIndex++)
            {
                AnimatorOverridePair overridePair = skin.animationOverrides[animOverrideIndex];
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
            return animatorOverrideController;
        }
    }
}
