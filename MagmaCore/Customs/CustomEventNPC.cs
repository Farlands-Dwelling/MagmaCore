using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Customs
{
    public abstract class CustomEventNPC : CustomBase
    {
        public EventNPC Instance;

        public virtual GameObject eventPrefab { get; protected set; }
        public virtual EventManager.EventType eventType { get; protected set; } = EventManager.EventType.None;
        public virtual List<string> farewells { get; protected set; }
        public virtual List<string> greetings { get; protected set; }
        public virtual List<RunType> invalidForRunTypes { get; protected set; }
        public virtual List<Character.CharacterName> validForCharacters { get; protected set; }

        public override void Convert()
        {
            EventNPC healerNPC = Resources.FindObjectsOfTypeAll<EventNPC>().ToList().Find(x => x.name == "Healer NPC Variant");
            EventNPC result = GameObject.Instantiate(healerNPC);//ScriptableObject.CreateInstance<Character>();
            Instance = result;

            if (eventPrefab != null) Instance.eventPrefab = eventPrefab;
            Instance.eventType = eventType;
            if (farewells != null) Instance.farewells = farewells;
            if (greetings != null) Instance.greetings = greetings;
            if (invalidForRunTypes != null) Instance.invalidForRunTypes = invalidForRunTypes;
            if (validForCharacters != null) Instance.validForCharacters = validForCharacters;
        }
    }
}
