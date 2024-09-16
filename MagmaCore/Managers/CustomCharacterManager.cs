using MagmaCore.Customs;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Managers
{
    public abstract class CustomCharacterManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance { get; private set; }

        protected GameFlowManager gameFlowManager;
        protected GameManager gameManager;

        public static Character.CharacterName CharacterName;
        public Character.CharacterName characterName => CharacterName;

        protected virtual void Awake()
        {
            if (instance != null)
            {
                MelonLogger.Error($"A {this.GetType()} instance already exists");
                Destroy(this); //Or GameObject as appropriate
                return;
            }
            instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        protected virtual void Start()
        {
            this.gameManager = GameManager.main;
            this.gameFlowManager = GameFlowManager.main;
        }

        public virtual void Spawn() { }
        public virtual void StartCombat() { }
        public virtual void EndCombat() { }
        public virtual void StartTurn() { }
        public virtual void EndTurn() { }
        public virtual void RemoveUI() { }
    }
}
