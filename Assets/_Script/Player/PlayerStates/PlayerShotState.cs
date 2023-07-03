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
            // Ray�������Ƀq�b�g�����ꍇ�̏���
            if (hit.collider.CompareTag("Player"))
            {
                // �q�b�g�����I�u�W�F�N�g��Player�^�O�����ꍇ�̏���
                Core pCore = hit.collider.gameObject.GetComponentInChildren<Core>();
                Damage damage = null;
                pCore.GetCoreComponent(ref damage);
                if (damage != null)
                {
                    //�_���[�W�����鏈��
                    damage.addDamage(1);
                    player.AddTeamCountServerRpc(player.nowTeam, 1);
                }
                else
                    Debug.Log(hit.collider.name + "��Damage�R���|�[�l���g��������܂���");
            }
        }

        stateMachine.ChangeState(player.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}