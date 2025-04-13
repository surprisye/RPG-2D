using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;
    
    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;
    private int facingDirection;
    
    private bool canDuplicateClone;
    private float chanceToDuplicate;
    
    private void Start()
    {
        facingDirection = 1;
        attackCheckRadius = .8f;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));
            if (sr.color.a < 0)
                Destroy(gameObject);
            
        }
    }

    public void SetupClone(Transform _newTransform,float _cloneDuration,
        bool _canAttack,Vector3 _offset,Transform _closestEnemy,
        bool _canDuplicate,float _chanceToDuplicate,Player _player)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber",Random.Range(1,3));
        }
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        player = _player;
        FaceClosestTarget();
    }
    
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                

                if (canDuplicateClone)
                {
                    if (Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform,
                            new Vector3(1.5f * facingDirection,0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDirection = -facingDirection;
                transform.Rotate(0, 180, 0);
            }
        }
        
    }
    
}
