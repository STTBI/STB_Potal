using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    protected PlayerController player;

    public PlayerState CurrentState { get; private set; }

    public void Initionalize(PlayerState InitState)
    {
        CurrentState = InitState;
    }

    public void ChangeState(PlayerState nextState)
    {
        if (CurrentState != null)
            CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }
}
