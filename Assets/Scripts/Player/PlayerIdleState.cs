using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    { 
        
    }

    public override void Enter()
    {
        base.Enter();
        
        player.SetZeroVelocity();//停止滑行
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    
}
