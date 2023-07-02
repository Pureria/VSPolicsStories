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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    #region Set Function
    public void SetRotation(Vector3 look)
    {
        workspace = look;
        SetFinalRotation();
    }

    public void SetRotation(float angle)
    {
        workspace = new Vector3(0.0f, angle, 0.0f);
        SetFinalRotateAngle();
    }

    private void SetFinalRotation()
    {
        if (CanSetRotate)
        {
            myTran.LookAt(workspace);
            CurrentRotate = workspace;
        }
    }

    private void SetFinalRotateAngle()
    {
        if (CanSetRotate)
        {
            myTran.Rotate(workspace);
            CurrentRotate = workspace;
        }
    }
    #endregion
}
