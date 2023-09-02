using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Customs
{
    public abstract class CustomEvent : CustomBase
    {
        public DungeonLevel.EventEncounter2 EventInstance;

        public virtual List<RunType.RunProperty.Type> disablingRunProperties { get; protected set; }
        /// <summary>
        /// The object that spawns when the event is triggered. This is typically an NPC.
        /// </summary>
        public virtual List<GameObject> eventType { get; protected set; }
        public virtual List<DungeonLevel.Floor> floor { get; protected set; }
        public virtual List<RunType.RunProperty.Type> requiredRunProperties { get; protected set; }
        public abstract List<MetaProgressSaveManager.MetaProgressCondition> storyModeConditions { get; protected set;}

        public override void Convert()
        {
            DungeonLevel.EventEncounter2 eventEncounter = new DungeonLevel.EventEncounter2();
            eventEncounter.disablingRunProperties = disablingRunProperties;
            eventEncounter.eventType = eventType;
            eventEncounter.floor = floor;
            eventEncounter.requiredRunProperties = requiredRunProperties;
            eventEncounter.storyModeConditions = storyModeConditions;

            EventInstance = eventEncounter;
        }
    }
}
