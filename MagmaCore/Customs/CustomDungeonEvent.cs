using MagmaCore.Utils;
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
        public GameObject Prefab;

        public virtual Vector2 caveIn { get; protected set; }
        public virtual string mapTextOverrideKey { get; protected set; } = "";
        public virtual bool passable { get; protected set; } = true;
        public virtual DungeonEvent.DungeonEventType dungeonEventType { get; protected set; } = DungeonEvent.DungeonEventType.Chance;
        /// <value>
        /// Determines what is spawned on the tile for this event. eg. Chest prefab
        /// </value>
        public virtual List<GameObject> itemsToSpawn { get; protected set; } = new List<GameObject>();
        public virtual GameObject exitPrefab { get; protected set; }
        public virtual Sprite iconSprite { get; protected set; }
        public virtual Sprite[] sprites { get; protected set; }
        public virtual List<GameObject> particles { get; protected set; } = new List<GameObject>();
        public virtual GameObject destroyParticles { get; protected set; }
        public virtual int turnsToExpire { get; protected set; } = -1;
        public virtual List<DungeonEvent.EventProperty> eventProperties { get; protected set; }
        public virtual int doorNumber { get; protected set; }

        public override void Convert()
        {
            GameObject dungeonEventOrig = Resources.FindObjectsOfTypeAll<DungeonEvent>().ToList().Find(x => x.gameObject.name == "Random Event").gameObject;
            Prefab = GameObject.Instantiate(dungeonEventOrig);
            Instance = Prefab.GetComponent<DungeonEvent>();

            Instance.gameObject.name = UniqueNameID;

            if (caveIn != null) Instance.caveIn = caveIn;
            if (mapTextOverrideKey != "") Instance.mapTextOverrideKey = mapTextOverrideKey;
            Instance.passable = passable;
            Instance.dungeonEventType = dungeonEventType;
            Instance.itemsToSpawn = itemsToSpawn;
            if (exitPrefab != null) Instance.exitPrefab = exitPrefab;
            if (iconSprite != null) Instance.GetComponent<SpriteRenderer>().sprite = iconSprite;
            if (sprites != null) Instance.sprites = sprites;
            if (particles != null) Instance.particles = particles;
            if (destroyParticles != null) Instance.destroyParticles = destroyParticles;
            Instance.turnsToExpire = turnsToExpire;
            if (eventProperties != null) Instance.eventProperties = eventProperties;
            Instance.doorNumber = doorNumber;

            Prefab.transform.SetParent(Main.Hider.transform);
            Modify(ref Instance);
        }

        public virtual void Modify(ref DungeonEvent dungeonEventInstance) { }
    }
}
