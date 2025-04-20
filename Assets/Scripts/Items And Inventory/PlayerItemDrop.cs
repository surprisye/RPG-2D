using System.Collections.Generic;
using UnityEngine;
public class PlayerItemDrop : ItemDrop
{
     [Header("Player`s Drop")] 
     [SerializeField] private float chanceToDropItems;

     [SerializeField] private float chanceToLooseMaterials;

     public override void GenerateDrop()
     {
          Inventory inventory = Inventory.instance;
          
          List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
          
          List<InventoryItem> materialsToDrop = new List<InventoryItem>();

          foreach (InventoryItem item in inventory.GetEquipmentList())
          {
               if (Random.Range(0,100) <= chanceToDropItems)
               {
                    
                    DropItem(item.data);
                    itemsToUnequip.Add(item);
               }
          }

          for (int i = 0; i < itemsToUnequip.Count; i++)
          {
               inventory.UnEquipItem(itemsToUnequip[i].data as ItemData_Equipment);
               
          }
          
          foreach (InventoryItem item in inventory.GetStashList())
          {
               if (Random.Range(0,100) <= chanceToLooseMaterials)
               {
                    DropItem(item.data);
                    materialsToDrop.Add(item);
               }
          }

          for (int i = 0; i < materialsToDrop.Count; i++)
          {
               inventory.RemoveItem(materialsToDrop[i].data);
          }
     }
}
