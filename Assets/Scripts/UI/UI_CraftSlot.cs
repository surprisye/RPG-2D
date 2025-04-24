using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetUpCraftSlot(ItemData_Equipment _data)
    {
        if (_data  == null)
            return;
        
        item.data = _data;
        
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;

        if (itemText.text.Length > 10)
        {
            itemText.fontSize = itemText.fontSize * 0.8f;
        }
        else
        {
            itemText.fontSize = 32;
        }
    }
    

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetUpCraftWindow(item.data as ItemData_Equipment);

    }
}
