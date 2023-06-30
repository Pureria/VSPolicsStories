using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody myRB { get; private set; }
    public bool CanSetVelocity { get; set; }
    public Vector3 CurrentVelocity { get; private set; }

    private Vector3 workspace;

    protected override void Start()
    {
        base.Start();

        if (!this.IsOwner)
            return;

        myRB = GetComponentInParent<Rigidbody>();
        CanSetVelocity = true;
        SetInitVariablesServerRpc();
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = myRB.velocity;
    }

    #region Set Function
    [Unity.Netcode.ServerRpc]
    public void SetVelocityZeroServerRpc()
    {
        workspace = Vector3.zero;
        SetFinalVelocity();
    }

    [Unity.Netcode.ServerRpc]
    public void SetVelocityServerRpc(Vector3 velocity, float speed)
    {
        workspace = velocity.normalized * speed;
        SetFinalVelocity();
    }

    [Unity.Netcode.ServerRpc]
    private void SetInitVariablesServerRpc()
    {
        myRB = GetComponentInParent<Rigidbody>();
        CanSetVelocity = true;
    }

    private void SetFinalVelocity()
    {
        if(IsServer)
        {
            if (CanSetVelocity)
            {
                myRB.velocity = workspace;
                CurrentVelocity = workspace;
            }
        }
    }

    #endregion
}
