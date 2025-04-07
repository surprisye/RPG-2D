using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState{ get; private set; }//可读不可改写

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
    
}
