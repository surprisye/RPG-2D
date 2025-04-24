using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EquipmentType
{
    Weapon,     //武器
    Armor,      //护甲
    Amulet,     //饰品
    Flask       //药品
}

[CreateAssetMenu(fileName = "New Equipment",menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    
    [Header("Unique Effect")]
    public float itemCoolDown;
    public ItemEffect[] itemEffects;



    [Header("Major Stats")]
    public int strength;       //1点strength升级百分之一的伤害和暴击伤害
    public int agility;        //增加闪避和暴击率
    public int intelligence;   //增加魔法攻击和魔法防御
    public int vitality;       //增加血量
    
    [Header("Offensive Stats")]
    public int damage;
    public int critChance;     //
    public int critPower;      //默认150%
    
    [FormerlySerializedAs("Health")] [FormerlySerializedAs("maxHealth")] [Header("Defensive Stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;
    
    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;
    
    [Header("Craft Requirement")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;
    public void ExecuteItemEffect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
    
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        
        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);
        
        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    } 
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        
        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);
        
        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;
        
        
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");
        
        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "CritChance");
        AddItemDescription(critPower, "CritPower");
        
        AddItemDescription(health, "Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "MagicResistance");
        AddItemDescription(fireDamage, "FireDamage");
        AddItemDescription(iceDamage, "IceDamage");
        AddItemDescription(lightingDamage, "LightingDamage");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.Append("Unique: -" + itemEffects[i].effectDescription);
                descriptionLength++;
            }
        }
        
        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append(" ");
            }
        }

        
        
        return sb.ToString();
    }
    

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            

            if (_value > 0)
                sb.Append("+ " + _value + " " + _name);

            descriptionLength++;
        }
    }
}
