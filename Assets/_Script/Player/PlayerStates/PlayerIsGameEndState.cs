using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsGameEndState : PlayerState
{
    public PlayerIsGameEndState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityZero();
        Movement.CanSetVelocity = false;
        Rotation.CanSetRotate = false;
    }

    public override void Exit()
    {
        base.Exit();

        Movement.CanSetVelocity = true;
        Rotation.CanSetRotate = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
