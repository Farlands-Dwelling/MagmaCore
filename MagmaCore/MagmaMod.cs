using MagmaCore.Customs;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaCore
{
    public abstract class MagmaMod : MelonMod
    {
        public static string LoadedScene;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            OnPostSceneWasLoaded(buildIndex, sceneName);
        }

        public virtual void OnPostSceneWasLoaded(int buildIndex, string sceneName)
        {
            LoadedScene = sceneName;
        }

        public T AddCharacter<T>() where T : CustomCharacter, new()
        {
            T character = new T();
            character.ModID = $"{Info.Author}.{Info.Name}"; //Probably not good long term, if mod author changes name of mod or author name, things will get messed up
            character.ModName = Info.Name;

            return CustomCharacter.RegisterCharacter(character);
        }
    }
}
