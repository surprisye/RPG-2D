using System;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    private UI_ItemToolTip ui_ItemToolTip;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;
    private void Start()
    {
        defaultFontSize = 32;
    }

    public void ShowToolTip(ItemData_Equipment _item)
    {
        if (_item == null)
            return;
        
        
        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescription.text = _item.GetDescription();

        AdjustFontSize(itemNameText);
        
        AdjustPosition();
        gameObject.SetActive(true);
    }
    
    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
