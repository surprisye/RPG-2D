using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
     
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    

    private void SetupVisuals()
    {
        if (itemData == null)
            return;
        
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object-" + itemData.itemName;
    }
    

    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.linearVelocity = _velocity;
        
        SetupVisuals();
    }

    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.linearVelocity = new Vector2(0, 7);
            return;
        }
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
