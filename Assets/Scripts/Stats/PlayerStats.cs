using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
    }

    protected override void Die()
    {
        base.Die();
        
        player.Die();
        
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        
        ItemData_Equipment currentArmour = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmour != null)
        {
            currentArmour.ExecuteItemEffect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        Debug.Log("玩家躲避了伤害");
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats,float _Multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats)) 
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();
        if (_Multiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _Multiplier);
        }

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        
        
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicDamage(_targetStats);//如果你不想在普通攻击上附加魔法伤害则删除
    }
}
