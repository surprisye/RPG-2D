using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    health,
    armor,
    evasion,
    magicResist,
    fireDamage,
    iceDamage,
    lightningDamage,
}

[CreateAssetMenu(fileName = "Buff Effect",menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _respondPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        stats.IncreaseStatBy(buffAmount,buffDuration,StatModify());
    }

    private Stat StatModify()
    {
        switch (buffType)
        {
            case StatType.strength:
                return stats.strength;
            case StatType.agility:
                return stats.agility;
            case StatType.intelligence:
                return stats.intelligence;
            case StatType.vitality:
                return stats.vitality;
            case StatType.damage:
                return stats.damage;
            case StatType.critChance:
                return stats.critChance;
            case StatType.health:
                return stats.maxHealth;
            case StatType.armor:
                return stats.armor;
            case StatType.evasion:
                return stats.evasion;
            case StatType.magicResist:
                return stats.magicResistance;
            case StatType.fireDamage:
                return stats.fireDamage;
            case StatType.iceDamage:
                return stats.iceDamage;
            case StatType.lightningDamage:
                return stats.lightingDamage;
            default:
                throw new System.ArgumentException("Unhandled stat type", "buffType");
        }
    }
}
