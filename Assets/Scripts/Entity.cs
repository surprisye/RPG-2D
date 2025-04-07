using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components

    public Animator anim { get;private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFx fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStatus status { get; private set; }

    #endregion

    [Header("Knockback Info")] 
    [SerializeField]protected Vector2 knockbackDirection;
    [SerializeField]protected float knockbackDuration;
    public bool isKnocked;
    
    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    
    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;
    
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        sr=GetComponentInChildren<SpriteRenderer>();
        fx = GetComponent<EntityFx>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<CharacterStatus>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFx");
        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        
        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDirection, knockbackDirection.y);
        
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
    
    #region Velocity

    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }
        
        rb.linearVelocity = new Vector2(0, 0);
        
    }
    
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    
    #endregion
    
    #region Collision
    public virtual bool ISGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool ISWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x,groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance,wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);
    }
    #endregion
    
    #region Filp
    
    public virtual void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    protected virtual void FlipController(float _x)
    {
        if (isKnocked)
        {
            return;
        }
        if (_x > 0 && !facingRight)
        {
            Flip();
        }else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    
    #endregion

    public void MakeTransparent(bool _isTransparent)
    {
        if (_isTransparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
}
