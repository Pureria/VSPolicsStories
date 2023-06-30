using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : CoreComponent
{
    public bool CanSetRotate { get; set; }
    public Vector3 CurrentRotate { get; private set; }

    private Transform myTran;

    private Vector3 workspace;

    protected override void Start()
    {
        base.Start();
        if (!this.IsOwner)
            return;

        myTran = transform.parent.parent;
        CanSetRotate = true;
        SetInitVariablesServerRpc();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    #region Set Function
    [Unity.Netcode.ServerRpc]
    public void SetRotationServerRpc(Vector3 look)
    {
        workspace = look;
        SetFinalRotation();
    }

    [Unity.Netcode.ServerRpc]
    public void SetRotationServerRpc(float angle)
    {
        workspace = new Vector3(0.0f, angle, 0.0f);
        SetFinalRotateAngle();
    }

    [Unity.Netcode.ServerRpc]
    private void SetInitVariablesServerRpc()
    {
        myTran = transform.parent.parent;
        CanSetRotate = true;
    }

    private void SetFinalRotation()
    {
        if(IsServer)
        {
            if (CanSetRotate)
            {
                myTran.LookAt(workspace);
                CurrentRotate = workspace;
            }
        }
    }

    private void SetFinalRotateAngle()
    {
        if(IsServer)
        {
            if (CanSetRotate)
            {
                myTran.Rotate(workspace);
                CurrentRotate = workspace;
            }
        }
    }
    #endregion
}
