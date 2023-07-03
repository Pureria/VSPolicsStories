using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : CoreComponent
{
    public bool isDamage { get; private set; }
    public int currentDamage { get; private set; }
    public bool canDamage { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        canDamage = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    #region SetFunction
    public void addDamage(int damage)
    {
        if(canDamage)
        {
            isDamage = true;
            currentDamage = damage;
        }
    }

    public void UseDamageFlg() => isDamage = false;
    public void SetCanDamage(bool flg) => canDamage = flg;
    #endregion
}
