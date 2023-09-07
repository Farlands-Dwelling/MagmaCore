using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace MagmaCore.Customs
{
    public abstract class CustomDungeonEvent : CustomBase
    {
        public DungeonEvent Instance;

        public virtual Vector2 caveIn { get; protected set; }
        public virtual string mapTextOverrideKey { get; protected set; } = "";
        public virtual bool passable { get; protected set; } = true;
        public virtual DungeonEvent.DungeonEventType dungeonEventType { get; protected set; }
        public virtual List<GameObject> itemsToSpawn { get; protected set; } = new List<GameObject>();
        public virtual GameObject exitPrefab { get; protected set; }
        public virtual Sprite[] sprites { get; protected set; }
        public virtual List<GameObject> particles { get; protected set; } = new List<GameObject>();
        public virtual GameObject destroyParticles { get; protected set; }
        public virtual int turnsToExpire { get; protected set; } = -1;
        public virtual List<DungeonEvent.EventProperty> eventProperties { get; protected set; }
        public virtual int doorNumber { get; protected set; }

        public override void Convert()
        {
            DungeonEvent dungeonEvent = UnityEngine.Object.Instantiate(Resources.FindObjectsOfTypeAll<DungeonEvent>().ToList().Find(x => x.gameObject.name == "Random Event").gameObject).GetComponent<DungeonEvent>();
            dungeonEvent.gameObject.name = UniqueNameID;

            if (caveIn != null) dungeonEvent.caveIn = caveIn;
            if (mapTextOverrideKey != "") dungeonEvent.mapTextOverrideKey = mapTextOverrideKey;
            //finish
        }
    }
}
