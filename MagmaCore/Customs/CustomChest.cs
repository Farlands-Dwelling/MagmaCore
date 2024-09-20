using MagmaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Customs
{
    public abstract class CustomChest : CustomDungeonEvent
    {
        public Chest ChestInstance;

        public virtual List<ItemSpawner.ItemToSpawn> chestItems { get; protected set; } = new List<ItemSpawner.ItemToSpawn> { };
        public virtual Sprite closedSprite { get; protected set; }
        public virtual Sprite openSprite { get; protected set; }
        public override DungeonEvent.DungeonEventType dungeonEventType => DungeonEvent.DungeonEventType.Chest;
        /// <value>
        /// Instead of using chestItems to automatically spawn items, overriding this and OnOpen allows you do to something else when this chest is opened.
        /// </value>
        public virtual bool useCustomOpenMethod { get; protected set; }
        public override void Modify(ref DungeonEvent dungeonEventInstance)
        {
            GameObject RecipeChestPrefab = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<Chest>()[0].transform.parent.gameObject);
            RecipeChestPrefab.name = "Recipe Chest";
            Transform spriteTransform = RecipeChestPrefab.transform.Find("NPC Sprite");
            spriteTransform.GetComponent<SpriteRenderer>().sprite = closedSprite;
            Chest chest = spriteTransform.GetComponent<Chest>();
            chest.openSprite = openSprite;
            chest.type = (Chest.Type)GetHash();

            RecipeChestPrefab.transform.SetParent(Main.Hider.transform);

            itemsToSpawn.Add(RecipeChestPrefab);
            ChestInstance = chest;
        }
        public virtual void OnOpen() { }
    }
}
