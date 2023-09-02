using MagmaCore.Customs;
using MagmaCore.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Utils
{
    public static class CharacterUtils
    {
        public static void AddItemToPool(this Character character, ref Item2 item)
        {
            item.validForCharacters.Add(character.characterName);
        }
        public static void AddItemToPool(this Character character, string itemName)
        {
            ItemUtils.FindItem(itemName).validForCharacters.Add(character.characterName);
        }
        public static void AddItemToPool(this Character character, ModItemDefinition itemDef)
        {
            ItemUtils.FindItem(itemDef).validForCharacters.Add(character.characterName);
        }

        public static CustomCharacter FindCustomCharacter(string characterName, string modName)
        {
            return CustomUtils.GetCastedCustom<CustomCharacter>(modName, characterName);
                //CustomBase.CustomsByModName[new KeyValuePair<string, string>(modName, characterName)];
        }

        public static Character FindCharacter(string characterName, string modName)
        {
            return FindCustomCharacter(characterName, modName).CharacterInstance;
        }

        public static Character FindCharacter(string characterName)
        {
            return Resources.FindObjectsOfTypeAll<Character>().ToList().Find(x => x.name.ToLower() == characterName.ToLower());
        }
    }
}
