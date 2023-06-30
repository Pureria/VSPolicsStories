using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class PlayerState
{
    protected Core core;

    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    protected int xInput;
    protected int zInput;

    protected bool dashinput;
    protected bool shotInput;
    protected bool reloadInput;
    protected bool interactInput;
    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected bool canMelee;
    protected bool isInteractUIShow = false;

    protected Vector3 workspace;

    private string animBoolName;

    private bool meleeInput;

    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Movement movement;
    protected Rotation Rotation{ get => rotation?? core.GetCoreComponent(ref rotation); }
    private Rotation rotation;

    public PlayerState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
        core = player.Core;
    }

    public virtual void Enter()
    {
        DoCheck();
        player.Anim.SetBool(animBoolName, true);
        isExitingState = false;
        canMelee = true;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        xInput = player.inputController.NormInputX;
        zInput = player.inputController.NormInputZ;
        shotInput = player.inputController.ShotInput;
        dashinput = player.inputController.DashInput;
        reloadInput = player.inputController.ReloadInput;
        interactInput = player.inputController.InteractInput;

        meleeInput = player.inputController.MeleeInput;
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void DoCheck() { }
    public virtual void AnimationTrigger() { }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
