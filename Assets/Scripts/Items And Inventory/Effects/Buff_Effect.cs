using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
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
        
        stats.IncreaseStatBy(buffAmount,buffDuration,stats.StatOfType(buffType));
    }

    
}
