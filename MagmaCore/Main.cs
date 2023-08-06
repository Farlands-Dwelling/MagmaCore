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
        public override void OnPostSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnPostSceneWasLoaded(buildIndex, sceneName);

            if (sceneName == "MainMenu")
            {
                foreach (CustomCharacter character in CustomCharacter.CustomCharacters)
                {
                    character.Convert();
                }
            }
        }
    }
}
