using MagmaCore.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

namespace MagmaCore.Customs
{
    public abstract class CustomDungeonEventSpawn : CustomBase
    {
        public DungeonSpawner.DungeonEventSpawn EventInstance;

        public virtual List<DungeonLevel> dungeonLevels { get; protected set; } = Resources.FindObjectsOfTypeAll<DungeonLevel>().ToList();
        /// <value>
        /// What floors will this event spawn on. For almost every event, it is recommended to add floors one (first), two (second) and for things like chests; the boss floor (boss).
        /// </value>
        public virtual List<DungeonLevel.Floor> floors { get; protected set; } = new List<DungeonLevel.Floor>() { DungeonLevel.Floor.first, DungeonLevel.Floor.second };
        public virtual List<RunType.RunProperty.Type> disablingRunProperties { get; protected set; } = new List<RunType.RunProperty.Type>();
        public virtual bool ignoreOnLastFloor { get; protected set; } = false;
        public virtual Vector2 num { get; protected set; } = Vector2.one;
        /// <value>
        /// The objects that spawn when the event is triggered.
        /// </value>
        public virtual List<GameObject> prefabList { get; protected set; } = new List<GameObject>();
        /// <value>
        /// The event will only show up in endless mode.
        /// </value>
        public virtual bool repeatLoopsOnly { get; protected set; } = false;
        public virtual List<RunType.RunProperty.Type> requiredRunProperties { get; protected set; } = new List<RunType.RunProperty.Type>();
        public virtual List<MetaProgressSaveManager.MetaProgressCondition> storyModeConditions { get; protected set; } = new List<MetaProgressSaveManager.MetaProgressCondition>();
        public virtual bool storyModeOnly { get; protected set; } = false;
        public virtual DungeonSpawner.DungeonEventSpawn.Type type { get; protected set; } = DungeonSpawner.DungeonEventSpawn.Type.mainPath;
        public virtual List<Character.CharacterName> validForCharacters { get; private set; } = new List<Character.CharacterName>();

        public override bool UniqueConversionMethod => true;

        public override void Convert()
        {
            EventInstance = new DungeonSpawner.DungeonEventSpawn();

            EventInstance.disablingRunProperties = disablingRunProperties;
            EventInstance.ignoreOnLastFloor = ignoreOnLastFloor;
            EventInstance.num = num;
            EventInstance.prefabList = prefabList;
            EventInstance.repeatLoopsOnly = repeatLoopsOnly;
            EventInstance.requiredRunProperties = requiredRunProperties;
            EventInstance.storyModeConditions = storyModeConditions;
            EventInstance.storyModeOnly = storyModeOnly;
            EventInstance.type = type;
            EventInstance.validForCharacters = validForCharacters;

            foreach(DungeonLevel dungeon in dungeonLevels)
            {
                foreach (DungeonLevel.Floor floor in floors)
                {
                    DungeonLevel.DungeonEventsToSpawn eventSpawn = dungeon.itemsToSpawnOnMap.Find(x => x.floor == floor);
                    if (eventSpawn == null)
                    {
                        eventSpawn = new DungeonLevel.DungeonEventsToSpawn()
                        {
                            floor = floor
                        };
                        dungeon.itemsToSpawnOnMap.Add(eventSpawn);
                        eventSpawn.itemsToSpawnOnMap = new List<DungeonSpawner.DungeonEventSpawn>();
                    }
                    if (eventSpawn.itemsToSpawnOnMap == null)
                        eventSpawn.itemsToSpawnOnMap = new List<DungeonSpawner.DungeonEventSpawn>();

                    eventSpawn.itemsToSpawnOnMap.Add(EventInstance);
                }
            }
        }
    }

    [HarmonyPatch(typeof(DungeonSpawner), "SetAllEncounters")]
    class DungeonSpawnerAwake_Patch
    {
        static bool Prefix(ref DungeonSpawner __instance)
        {
            foreach (CustomBase custom in CustomBase.Customs.Values)
            {
                if (custom is CustomDungeonEventSpawn)
                    custom.Convert();
            }
            return true;
        }
    }
}
