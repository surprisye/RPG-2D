using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop itemDropSystem;

    [Header("level details")] 
    [SerializeField] private int level;
    
    [Range(0f,1f)]
    [SerializeField] private float percentageModifier = .4f;
    
    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        itemDropSystem = GetComponent<ItemDrop>();
        
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);
        
        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);
        
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(maxHealth);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        
        itemDropSystem.GenerateDrop();
    }
}
