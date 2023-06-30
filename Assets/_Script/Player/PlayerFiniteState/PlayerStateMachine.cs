using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public bool canChangeState;
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState iniState)
    {
        canChangeState = true;
        CurrentState = iniState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState nextState)
    {
        if (!canChangeState)
            return;

        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }
}
