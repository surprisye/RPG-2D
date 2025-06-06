using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.skill.dash.CloneOnDash();

        stateTimer = player.dashDuration;
        
        
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(player.dashSpeed * player.dashDirection,rb.linearVelocity.y);
        
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.ISWallDetected() && !player.ISGroundDetected())
        {
            stateMachine.ChangeState(player.wallSlide);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.skill.dash.CloneOnDashArrival();

        player.SetVelocity(0,rb.linearVelocity.y);
        
    }
}
