using MagmaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Customs
{
    public abstract class CustomStatusEffect : CustomBase
    {
        public StatusEffect.Type StatusEffectInstance;
        public Item2.Effect.Type Item2EffectInstance;
        // TODO: add sfx maybe? patch Status.ApplyStatusEffect

        public virtual TranslationUtils.Translation Translations { get; protected set; }
        public virtual Sprite Sprite { get; protected set; }
        public virtual bool IsNumeric { get; protected set; }
        public virtual StatusEffect.DecreasesTime DecreasesTimeType { get; protected set; }

        public override void Convert()
        {
            StatusEffectInstance = (StatusEffect.Type)GetHash();
            Item2EffectInstance = (Item2.Effect.Type)StatusEffectInstance;
        }
    }
}
