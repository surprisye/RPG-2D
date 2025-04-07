using UnityEngine;
using UnityEngine.Serialization;

public class BlackHole_Skill : Skill
{
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCoolDown;
    [SerializeField] private float blackHoleDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    private BlackHole_Skill_Controller currentBlackHole;
    
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    
    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = 
            Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);

        currentBlackHole = 
            newBlackHole.GetComponent<BlackHole_Skill_Controller>();
        
        currentBlackHole.SetupBlackHole(maxSize, growSpeed,
            shrinkSpeed,amountOfAttacks,cloneAttackCoolDown,blackHoleDuration);
    }

    public bool BlackHoleSkillCompleted()
    {
        if (!currentBlackHole)
        {
            return false;
        }
        
        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }
        
        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    
    

    
}
