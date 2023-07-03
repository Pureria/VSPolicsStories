using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody myRB { get; private set; }
    public bool CanSetVelocity { get; set; }
    public Vector3 CurrentVelocity { get; private set; }

    private Vector3 workspace;

    protected override void Awake()
    {
        base.Awake();

        //if (!this.IsOwner)
            //return;

        CanSetVelocity = true;
        myRB = GetComponentInParent<Rigidbody>();
        //SetInitVariables();
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = myRB.velocity;
        //SetPositionServerRpc(myTran.position);
    }

    #region Set Function
    public void SetVelocityZero()
    {
        workspace = Vector3.zero;
        SetFinalVelocity();
    }

    public void SetVelocity(Vector3 velocity, float speed)
    {
        workspace = velocity.normalized * speed;
        SetFinalVelocity();
    }


    private void SetFinalVelocity()
    {
        if (CanSetVelocity)
        {
            myRB.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }

    /*
    [Unity.Netcode.ServerRpc]
    private void SetInitVariables()
    {
        myRB = GetComponentInParent<Rigidbody>();
        myTran = GetComponentInParent<Transform>();
        CanSetVelocity = true;
    }
    */

    /*
    [Unity.Netcode.ServerRpc]
    private void SetPositionServerRpc(Vector3 pos)
    {
        myTran.position = pos;
    }
    */
    #endregion
}
