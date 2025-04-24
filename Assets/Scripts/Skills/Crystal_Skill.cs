using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;
    
    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;
    
    [Header("Crystal Simple")] 
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked {get;private set;}
    
    [Header("Explosive crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;
    
    
    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }

    #region Unlock Skill Region
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }

    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
        {
            cloneInsteadOfCrystal = true;
        }
    }

    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
        {
            canExplode = true;
        }
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }

    private void UnlockMultiStack()
    {
        if (unlockMultiStackButton.unlocked)
        {
            canUseMultiStacks = true;
        }
    }
    #endregion

    public override bool CanUseSkill()
    {
        
        if (cooldownTimer < 0)
        {
            UseSkill();
            
            return true; 
        }
        
        return false;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        
        cooldownTimer = cooldown;

        if (CanUseMultiCrystal())
        {
            cooldownTimer = cooldown;
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrystal();
            cooldownTimer = 0;
        }
        else
        {
            
            if (canMoveToEnemy)
                return;
            
            
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform,Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
            
            cooldownTimer = cooldown;
        }
        
        
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab,player.transform.position,Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            
        currentCrystalScript.SetupCrystal(crystalDuration,canExplode,
            canMoveToEnemy, moveSpeed,FindClosestEnemy(currentCrystal.transform),player);
        currentCrystalScript.ChooseRandomEnemy();
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();
    }
    
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility",useTimeWindow);
                
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn,
                    player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);
                
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration,canExplode, canMoveToEnemy,
                        moveSpeed,FindClosestEnemy(newCrystal.transform),player);
                
                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }

                
                return true;
            }
            
        }
        
        
        return false;
    }
    
    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {

        if (cooldownTimer > 0)
            return;
            
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
