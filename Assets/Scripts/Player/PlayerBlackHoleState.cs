using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime;
    private bool skillUsed;

    private float defaultGravity;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        defaultGravity = rb.gravityScale;
        flyTime = .4f;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0, 15);
        if (stateTimer < 0)
        {
            rb.linearVelocity = new Vector2(0, -.1f);
            if (!skillUsed)
            {
                if (player.skill.blackHole.CanUseSkill())
                {
                    skillUsed = true;
                }
                
            }
        }
        
        //退出状态在黑洞攻击完成后在skill_Controller
        if (player.skill.blackHole.BlackHoleSkillCompleted())
        {
            stateMachine.ChangeState(player.airState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        
        rb.gravityScale = defaultGravity;
        player.fx.MakeTransparent(false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
