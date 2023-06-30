using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoreComponent :  NetworkBehaviour, ILogicUpdate
{
    protected Core core;

    protected virtual void Start()
    {
        if (!this.IsOwner)
            return;

        core = transform.parent.GetComponent<Core>();

        if (core == null) { Debug.LogError("Coreスクリプトが見つかりません。"); }
        core.AddCoreComponent(this);
    }

    public virtual void LogicUpdate() { }
}
