using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{

    private EntityFx fx;
    
    [Header("Major Stats")]
    public Stat strength;       //1点strength升级百分之一的伤害和暴击伤害
    public Stat agility;        //增加闪避和暴击率
    public Stat intelligence;   //增加魔法攻击和魔法防御
    public Stat vitality;       //增加血量
    
    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;     //
    public Stat critPower;      //默认150%
    
    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;
    
    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited;      //做燃烧伤害
    public bool isChilled;      //减少护甲
    public bool isShocked;      //减少准确率

    [SerializeField] private float alimentsDuration;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    
    [SerializeField] private float igniteDamageCoolDown;
    private float igniteDamageTimer;
    private int igniteDamage;
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    
    public int currentHealth;
    
    public System.Action onHealthChanged;
    public bool isDead { get;private set; }
    public bool isVulnerable { get;private set; }

    protected virtual void Start()
    {
        alimentsDuration = 4;
        igniteDamageCoolDown = .3f;
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        
        fx = GetComponent<EntityFx>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime; 
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;
        if (isIgnited)
            ApplyIgniteDamage();
    }

    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableForCoroutine(_duration));
    }
    
    private IEnumerator VulnerableForCoroutine(float _duration)
    {
        isVulnerable = true;
        
        yield return new WaitForSeconds(_duration);
        
        isVulnerable = false;
    }
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statOfType)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statOfType));
    }

    IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statOfType)
    {
        _statOfType.AddModifier(_modifier);
        
        yield return new WaitForSeconds(_duration);
        
        _statOfType.RemoveModifier(_modifier);
    }
    
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats)) 
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        
        
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicDamage(_targetStats);//如果你不想在普通攻击上附加魔法伤害则删除
    }

    #region Magical Damage and Aliments
    
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightingDamage.GetValue();
        
        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage,_iceDamage,_lightningDamage) <= 0)
            return;
        
        AttemptToApplyAliments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AttemptToApplyAliments(CharacterStats _targetStats, int _fireDamage, int _iceDamage,
        int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill  = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock  = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyShock && !canApplyChill && !canApplyIgnite)
        {
            // 生成两次独立随机位（0或1）
            int bit1 = Random.value < 0.5f ? 0 : 1;
            int bit2 = Random.value < 0.5f ? 0 : 1;
            int combined = (bit1 << 1) | bit2;  // 结果为 0/1/2/3

            // 舍弃无效组合（如combined=3），有效组合映射到0-2
            if (combined == 3) continue; 

            switch (combined)
            {
                case 0 when _fireDamage > 0:    // 00 → Ignite
                    canApplyIgnite = true;
                    break;
                case 1 when _iceDamage > 0:     // 01 → Chill
                    canApplyChill = true;
                    break;
                case 2 when _lightningDamage > 0:  // 10 → Shock
                    canApplyShock = true;
                    break;
                default:
                    continue;
            }
            if (canApplyIgnite || canApplyChill || canApplyShock)
            {
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        if (canApplyShock)
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightningDamage * .1f));
        
        
        _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilment(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = alimentsDuration;
            
            fx.IgniteFxFor(alimentsDuration);
        }

        if (_chill && canApplyChill)
        {
            chilledTimer = alimentsDuration;
            isChilled = _chill;
            
            GetComponent<Entity>().SlowEntity(.2f,alimentsDuration);
            fx.ChillFxFor(alimentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                

                HitNearestTargetWithShockStrike();
            }
           
        }
        
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;
        
        
        shockedTimer = alimentsDuration;
        isShocked = _shock;
                
        fx.ShockFxFor(alimentsDuration);
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0 && isIgnited)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0)
            {
                Die();
            }
            
            igniteDamageTimer = igniteDamageCoolDown;
        }
    }
    
    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position,hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, 
                    hit.transform.position);
                
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage,closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockDamage(int _damage) => shockDamage = _damage;

    
    #endregion
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFx");

        if (currentHealth <= 0 &&!isDead)
        {
            Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        
        if(currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();
        
        onHealthChanged?.Invoke();
        Debug.Log("治疗了" + _amount);
    }
    
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage * 1.1f);
        }
        currentHealth -= _damage;
        
        onHealthChanged?.Invoke();
        
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    #region Start Calculations
    
    protected  int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    
    private  int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    { // 检查目标对象是否有效
        if (_targetStats == null || _targetStats.gameObject == null) {
            Debug.LogError("目标角色状态无效！可能已被销毁或未初始化。");
            return 0;
        }

        // 检查魔法抗性属性是否已初始化
        if (_targetStats.magicResistance == null) {
            Debug.LogError($"{_targetStats.gameObject.name} 的魔法抗性未配置！");
            return 0;
        }
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public virtual void OnEvasion()
    {
        
    }
    
    protected  bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue() + _targetStats.vitality.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }
        
        if (Random.Range(0,100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0,100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritDamage = (critPower.GetValue() + strength.GetValue()) * 0.1f;
        
        float critDamage = _damage + totalCritDamage;
        
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #endregion
    
    public Stat StatOfType(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agility:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.critChance:
                return critChance;
            case StatType.critPower:
                return critPower;
            case StatType.health :
                return maxHealth;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicResist:
                return magicResistance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightningDamage:
                return lightingDamage;
            default:
                throw new System.ArgumentException("Unhandled stat type", "_statType");
        }
    }
}
