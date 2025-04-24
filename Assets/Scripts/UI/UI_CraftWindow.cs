using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;
    
    [SerializeField] private Image[] materialsImage;

    public void SetUpCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();
        
        for (int i = 0; i < materialsImage.Length; i++)
        {
            materialsImage[i].color = Color.clear;
            materialsImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialsImage.Length)
            {
                Debug.Log("你设置的所需材料类型大于制造台的数量");
            }
            materialsImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialsImage[i].color = Color.white;

            TextMeshProUGUI MaterialsSlotText = materialsImage[i].GetComponentInChildren<TextMeshProUGUI>();
            
            MaterialsSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            MaterialsSlotText.color = Color.white;
        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();
        
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data,_data.craftingMaterials));
    }
}
