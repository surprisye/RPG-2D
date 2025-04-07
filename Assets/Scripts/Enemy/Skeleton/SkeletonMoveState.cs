using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void Update()
    {
        base.Update();
        
        enemy.SetVelocity( enemy.moveSpeed * enemy.facingDirection,rb.linearVelocity.y);

        if (enemy.ISWallDetected() || !enemy.ISGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
            
        }
    }
}
