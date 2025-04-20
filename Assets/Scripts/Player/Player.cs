using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")] 
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    
    
    public bool isBusy { get; private set; }
    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDirection{get;private set;}


    public SkillManager skill {get;private set;}
    public GameObject sword {get;private set;}
    
    #region State

    public PlayerStateMachine stateMachine { get;private set; }
    public PlayerState idleState { get;private set; }
    public PlayerState moveState { get;private set; }
    public PlayerState jumpState { get;private set; }
    public PlayerState airState { get;private set; }
    public PlayerDashState dashState { get;private set; }
    public PlayerWallSlideState wallSlide { get;private set; }
    public PlayerWallJumpState wallJump { get;private set; }
    public PlayerPrimaryAttackState primaryAttackState { get;private set; }
    public PlayerCounterAttackState counterAttackState { get;private set; }
    public PlayerCatchSwordState catchSword { get;private set; }
    public PlayerAimSwordState aimSword { get;private set; }
    public PlayerBlackHoleState blackHole{get;private set;}
    public PlayerDeadState deadState { get;private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        stateMachine = gameObject.AddComponent<PlayerStateMachine>();
        
        //移动机制
        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this,stateMachine,"Move");
        jumpState = new PlayerJumpState(this,stateMachine,"Jump");
        airState  = new PlayerAirState(this,stateMachine,"Jump");
        dashState = new PlayerDashState(this,stateMachine,"Dash");
        wallSlide = new PlayerWallSlideState(this,stateMachine,"WallSlide");
        wallJump = new PlayerWallJumpState(this,stateMachine,"Jump");
        
        //攻击机制
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        //技能
        aimSword = new PlayerAimSwordState(this,stateMachine,"AimSword");
        catchSword = new PlayerCatchSwordState(this,stateMachine,"CatchSword");
        blackHole = new PlayerBlackHoleState(this, stateMachine, "Jump");
        
        deadState = new PlayerDeadState(this,stateMachine,"Die");
    }

    protected override void Start()
    {
        base.Start();
        
        skill = SkillManager.instance;
        
        stateMachine.Initialize(idleState);
        
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
        
    }
    
    protected override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
        
        CheckDashInput();

        if (Input.GetKeyDown(KeyCode.F))
            skill.crystal.CanUseSkill();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("用血瓶");
            Inventory.instance.UseFlask();
        }

    }

    public override void SlowEntity(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        
        Invoke("ReturnDefaultSpeed" ,_slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    /*public void ExitBlackHoleAbility()
    {
        stateMachine.ChangeState(airState);
    }*/
    
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        
        yield return new WaitForSeconds(_seconds);
        
        isBusy = false;
    }
    
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckDashInput()
    {
        if (ISWallDetected() && !ISGroundDetected())
            return;
        
        

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }
            
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        
        stateMachine.ChangeState(deadState);
    }
}
