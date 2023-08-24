using MagmaCore.Customs;
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
        public static readonly List<Character> Characters = new List<Character>();
        public static readonly int ExtraItemFunctionNum = 555; //TODO: placeholder, should definitely find a better way than using numbers in jsons
        public override void OnFirstMainMenuLoad()
        {
            foreach (CustomCharacter character in CustomCharacter.CustomCharacters.Values)
            {
                character.Convert();
                Characters.Add(character.CharacterInstance);
            }
        }

        public override void OnPostSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Game")
            {
                foreach (MonoBehaviour manager in CustomCharacter.CharacterManagers)
                {
                    GameObject.FindObjectOfType<GameManager>().gameObject.AddComponent(manager.GetType());
                }
            }
        }
    }
}
