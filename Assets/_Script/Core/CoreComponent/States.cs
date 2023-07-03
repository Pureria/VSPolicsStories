using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States : CoreComponent
{
    public bool setInitFunction { get; private set; }
    public int nowHP { get; private set; }
    public int maxHP { get; private set; }

    public bool isDead { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        setInitFunction = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    #region SetFunction
    public void InitState(int MaxHP)
    {
        this.maxHP = MaxHP;
        this.nowHP = maxHP;
        isDead = false;
        setInitFunction = true;
    }

    public void addDamage(int damage)
    {
        this.nowHP -= damage;

        if (this.nowHP <= 0)
            isDead = true;
    }

    public void SetNowHp(int nowHP)
    {
        this.nowHP = nowHP;
    }
    #endregion
}
