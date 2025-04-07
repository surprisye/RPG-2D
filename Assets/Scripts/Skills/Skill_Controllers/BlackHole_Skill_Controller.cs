using System;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPreFab;
    [SerializeField] private List<KeyCode> keyCodeList;
    
    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    private float blackHoleTimer;
    
    private bool canGrow = true;
    private bool canShrink;

    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;
    public int amountOfAttacks;
    public float cloneAttackCoolDown;
    private float cloneAttackTimer;
    private bool playerCanDisappear;
    
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createHotKey = new List<GameObject>();
    
    public bool playerCanExitState{get; private set;}

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed,
        int _amountOfAttacks,float _cloneAttackCoolDown,float _blackHoleDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCoolDown = _cloneAttackCoolDown;

        blackHoleTimer = _blackHoleDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
        {
            playerCanDisappear = false;
        }
    }

    public void Awake()
    {
        playerCanDisappear = true;
        canGrow = true;
        canCreateHotKeys = true;
    }
    private void Update()
    {
        
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if (blackHoleTimer < 0)
        {
            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
            
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        
        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(maxSize,maxSize),Time.deltaTime * growSpeed);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(-1,-1),Time.deltaTime * shrinkSpeed);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
        {
            return;
        }
        
        DestroyHotKey();
        cloneAttackReleased = true;
        canCreateHotKeys = false;
        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.MakeTransparent(true);
            
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCoolDown;
            
            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 1;
            else
                xOffset = -1;
            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex],new Vector3(xOffset,0));
            }
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                //FinishBlackHoleAbility();
                Invoke("FinishBlackHoleAbility",.5f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKey();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
        //PlayerManager.instance.player.ExitBlackHoleAbility();
    }

    private void DestroyHotKey()
    {
        if (createHotKey.Count <= 0 )
        {
            return;
        }

        for (int i = 0; i < createHotKey.Count; i++)
        {
            Destroy(createHotKey[i]);
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            return;
        }

        if (!canCreateHotKeys)
        {
            return;
        }
        
        GameObject newHotKey = Instantiate(hotKeyPreFab,
            collision.transform.position + new Vector3(0, 2),
            Quaternion.identity);
            
        createHotKey.Add(newHotKey);
        
        KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chooseKey);
        
        BlackHole_HotKey_Controller newHotKeyScript = 
            newHotKey.GetComponent<BlackHole_HotKey_Controller>();
        
        newHotKeyScript.SetupHotKey(chooseKey,collision.transform,this);

        //newHotKey.GetComponent<BlackHole_HotKey_Controller>().SetupHotKey();
        //respawn prefab of a hot key above enemy
        //targets.Add(collision.transform);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
