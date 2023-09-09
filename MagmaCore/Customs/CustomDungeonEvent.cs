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

        public abstract GameObject prefabToCopyOnto { get; }
        public virtual Vector2 caveIn { get; protected set; }
        public virtual string mapTextOverrideKey { get; protected set; } = "";
        public virtual bool passable { get; protected set; } = true;
        public virtual DungeonEvent.DungeonEventType dungeonEventType { get; protected set; }
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
            // CHANGE SO IT COPIES ONTO prefab
            DungeonEvent dungeonEventOrig = Resources.FindObjectsOfTypeAll<DungeonEvent>().ToList().Find(x => x.gameObject.name == "Random Event").gameObject.GetComponent<DungeonEvent>();
            DungeonEvent dungeonEvent = prefabToCopyOnto.AddComponent<DungeonEvent>().GetCopyOf(dungeonEventOrig);
            prefabToCopyOnto.AddComponent<SpriteRenderer>().GetCopyOf(dungeonEventOrig.gameObject.GetComponent<SpriteRenderer>());
            UnityEngine.Transform.Instantiate(dungeonEventOrig.gameObject.GetComponentInChildren<TextMeshPro>(true).transform).SetParent(prefabToCopyOnto.transform);
            //DungeonEvent dungeonEvent = dungeonEventOrig.gameObject.DuplicateOnto(prefabToCopyOnto).GetComponent<DungeonEvent>();

            dungeonEvent.gameObject.name = UniqueNameID;

            if (caveIn != null) dungeonEvent.caveIn = caveIn;
            if (mapTextOverrideKey != "") dungeonEvent.mapTextOverrideKey = mapTextOverrideKey;
            dungeonEvent.passable = passable;
            dungeonEvent.itemsToSpawn = itemsToSpawn;
            if (exitPrefab != null) dungeonEvent.exitPrefab = exitPrefab;
            if (iconSprite != null) dungeonEvent.GetComponent<SpriteRenderer>().sprite = iconSprite;
            if (sprites != null) dungeonEvent.sprites = sprites;
            if (particles != null) dungeonEvent.particles = particles;
            if (destroyParticles != null) dungeonEvent.destroyParticles = destroyParticles;
            dungeonEvent.turnsToExpire = turnsToExpire;
            if (eventProperties != null) dungeonEvent.eventProperties = eventProperties;
            dungeonEvent.doorNumber = doorNumber;

            dungeonEvent.gameObject.tag = dungeonEventOrig.gameObject.tag;
        }
    }
}
