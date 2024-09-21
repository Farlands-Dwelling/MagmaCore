using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static DungeonLevel;

namespace MagmaCore.Customs
{
    public abstract class CustomEventEncounter : CustomBase
    {
        public DungeonLevel.EventEncounter2 Instance;

        public virtual List<DungeonLevel> dungeonLevels { get; protected set; } = Resources.FindObjectsOfTypeAll<DungeonLevel>().ToList();
        public virtual List<RunType.RunProperty.Type> disablingRunProperties { get; protected set; } = new List<RunType.RunProperty.Type>();
        /// <summary>
        /// The object that spawns when the event is triggered. This is typically an NPC.
        /// </summary>
        public virtual List<GameObject> eventType { get; protected set; } = new List<GameObject>();
        public virtual List<DungeonLevel.Floor> floor { get; protected set; } = new List<DungeonLevel.Floor>();
        public virtual List<RunType.RunProperty.Type> requiredRunProperties { get; protected set; } = new List<RunType.RunProperty.Type>();
        public virtual List<MetaProgressSaveManager.MetaProgressCondition> storyModeConditions { get; protected set; } = new List<MetaProgressSaveManager.MetaProgressCondition>();
        public virtual float weight { get; protected set; } = 1f;
        public virtual bool storyModeOnly { get; protected set; } = false;
        public override bool ConvertOnGameLoad => true;

        public override void Convert()
        {
            DungeonLevel.EventEncounter2 eventEncounter = new DungeonLevel.EventEncounter2();
            eventEncounter.disablingRunProperties = disablingRunProperties;
            eventEncounter.eventType = eventType;
            eventEncounter.floor = floor;
            eventEncounter.requiredRunProperties = requiredRunProperties;
            eventEncounter.storyModeConditions = storyModeConditions;
            eventEncounter.weight = weight;
            eventEncounter.storyModeOnly = storyModeOnly;

            foreach (DungeonLevel dungeon in dungeonLevels)
            {
                if (!dungeon.eventEncounters.Contains(eventEncounter))
                    dungeon.eventEncounters.Add(eventEncounter);
            }

            Instance = eventEncounter;

        }
    }
}
