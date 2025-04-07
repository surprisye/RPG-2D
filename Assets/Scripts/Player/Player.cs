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
    
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
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
    public PlayerPrimaryAttackState PrimaryAttackState { get;private set; }
    public PlayerCounterAttackState CounterAttackState { get;private set; }
    public PlayerCatchSwordState catchSword { get;private set; }
    public PlayerAimSwordState aimSword { get;private set; }
    public PlayerBlackHoleState blackHole{get;private set;}
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
        PrimaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        CounterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        //技能
        aimSword = new PlayerAimSwordState(this,stateMachine,"AimSword");
        catchSword = new PlayerCatchSwordState(this,stateMachine,"CatchSword");
        blackHole = new PlayerBlackHoleState(this, stateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();
        
        skill = SkillManager.instance;
        
        stateMachine.Initialize(idleState);
        
    }
    
    protected override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
        
        CheckDashInput();

        if (Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }
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
}
