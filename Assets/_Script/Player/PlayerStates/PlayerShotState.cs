using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotState : PlayerState
{
    public PlayerShotState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Ray ray = new Ray(player.transform.position, player.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Rayが何かにヒットした場合の処理
            if (hit.collider.CompareTag("Player"))
            {
                // ヒットしたオブジェクトがPlayerタグを持つ場合の処理
                Core pCore = hit.collider.gameObject.GetComponentInChildren<Core>();
                Damage damage = null;
                pCore.GetCoreComponent(ref damage);
                if (damage != null)
                {
                    //ダメージが入る処理
                    damage.addDamage(1);
                    player.AddTeamCountServerRpc(player.nowTeam, 1);
                }
                else
                    Debug.Log(hit.collider.name + "のDamageコンポーネントが見つかりません");
            }
        }

        stateMachine.ChangeState(player.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}