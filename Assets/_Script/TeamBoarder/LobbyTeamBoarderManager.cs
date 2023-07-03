using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyTeamBoarderManager : NetworkBehaviour
{
    [SerializeField]
    private LobbyTeamBoarder redTeamBoarder;
    [SerializeField]
    private LobbyTeamBoarder blueTeamBoarder;
    [SerializeField]
    private float waitTime = 3.0f;

    private bool isWait;
    private float startTime;

    private void Start()
    {
        isWait = false;
    }

    private void Update()
    {
        //�T�[�o�[���̂ݏ��������s
        if (!IsServer) return;

        if(redTeamBoarder.onPlayer && blueTeamBoarder.onPlayer && !isWait)
        {
            isWait = true;
            startTime = Time.time;
            Debug.Log("�Q�[���J�n�܂�" + waitTime + "�b�O");
        }

        if(isWait)
        {
            if (!redTeamBoarder.onPlayer || !blueTeamBoarder.onPlayer)
                isWait = false;

            if(startTime + waitTime <= Time.time)
            {
                redTeamBoarder.player.SetPlayerServerRpc();
                blueTeamBoarder.player.SetPlayerServerRpc();
                isWait = false;
            }
        }
    }
}
