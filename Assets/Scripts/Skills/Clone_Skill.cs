using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePreFab;
    [SerializeField] private float cloneDuration;
    [Space] 
    
    [Header("Clone Attack")] 
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;
    
    [Header("Aggressive Clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [FormerlySerializedAs("aggressiveCloneAttackMultiplier")] [SerializeField] private float aggressiveAttackMultiplier;
    public bool canApplyOnHitEffect{get;private set;}
    
    
    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private bool canMultipleClone;
    [SerializeField] private float chanceToDuplicate;
    
    [Header("Crystal Instead Of Clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();
        
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockMultiClone();
        UnlockCrystalInsteadOfClone();
    }

    #region Unlock Region

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            attackMultiplier = cloneAttackMultiplier;
            canAttack = true;
        }
    }
    
    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            attackMultiplier = aggressiveAttackMultiplier;
            canApplyOnHitEffect = true;
        }
    }

    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            attackMultiplier = multipleCloneAttackMultiplier;
            canMultipleClone = true;
        }
    }

    private void UnlockCrystalInsteadOfClone()
    {
        if (crystalInsteadUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }

    #endregion
    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePreFab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,
            cloneDuration,canAttack,_offset,FindClosestEnemy(newClone.transform),
            canMultipleClone,chanceToDuplicate,player,attackMultiplier);
    }



    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform ,
            new Vector3(1 * player.facingDirection,0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform,_offset);
    }
}
