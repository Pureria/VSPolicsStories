using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManagerControll : NetworkBehaviour
{
    public static GameManagerControll Singleton;

    [SerializeField]
    private PlayerSpriteData playerSpriteData;

    public PlayerController[] PlayerArray { get; private set; } = new PlayerController[2];
    private bool[] PlayerDeadFlg;
    private bool[] PlayerRestartInput;
    public bool isGameEnd { get; private set; }
    public bool isGameNow { get; private set; }

    //TODO::���̏������̓v���C���[��HP3�Œ�̏ꍇ�ɂ݂̂����Ɠ����܂�
    //HP���ςɂ���ɂ̓X���C�_�[����UI���쐬���Ă�������
    private int RedTeamCount = 0;
    private int BlueTeamCount = 0;

    [SerializeField]
    private Transform LobbyTransform;
    [SerializeField]
    private Transform RedTeamSpawnTransform;
    [SerializeField]
    private Transform BlueTeamSpawnTransform;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            GameObject.Destroy(this);

        isGameEnd = false;

        PlayerDeadFlg = new bool[PlayerArray.Length];
        PlayerRestartInput = new bool[PlayerArray.Length];

        for (int i = 0; i < PlayerArray.Length; i++)
        {
            PlayerRestartInput[i] = false;
            PlayerDeadFlg[i] = false;
        }
    }

    private void Update()
    {
        //�T�[�o�[�̎��ȊO�������Ȃ�
        if (!this.IsServer)
        {
            //�T�[�o�[����Ȃ��Ƃ��̏���
        }
        else
        {
            //�T�[�o�[�̎��̏���
            if(isGameEnd)
            {
                bool flg = true;
                for(int i = 0;i<PlayerRestartInput.Length;i++)
                {
                    if (!PlayerRestartInput[i])
                        flg = false;
                }
                if(flg)
                {
                    //TODO::���X�^�[�g����
                    Debug.Log("�S�������X�^�[�g��I�����܂���");
                    isGameEnd = false;
                    PlayerController[] oldPlayer = new PlayerController[PlayerArray.Length];
                    for(int i= 0;i<PlayerArray.Length;i++)
                    {
                        oldPlayer[i] = PlayerArray[i];
                        PlayerArray[i] = null;
                        PlayerRestartInput[i] = false;
                        PlayerDeadFlg[i] = false;
                    }

                    for(int i = 0;i<oldPlayer.Length;i++)
                    {
                        oldPlayer[i].RestartClientRpc();
                    }
                    RedTeamCount = 0;
                    BlueTeamCount = 0;
                    SetTeamCountClientRpc(RedTeamCount, BlueTeamCount);
                }
            }
            else
            {
                for(int i = 0;i<PlayerDeadFlg.Length;i++)
                {
                    if (PlayerDeadFlg[i])
                    {
                        isGameEnd = true;
                        //HP���Ȃ��Ȃ������̏���
                        for (int j = 0; j < PlayerArray.Length; j++)
                        {
                            PlayerArray[j].PlayerGameEndClientRpc();
                        }
                    }
                }
            }

            //�Q�[��������Ȃ��ꍇ
            if(!isGameNow)
            {
                //�v���C���[����l���܂������_�ŃQ�[���J�n
                bool flg = true;
                foreach(PlayerController player in PlayerArray)
                {
                    if (player == null)
                        flg = false;
                }

                if(flg)
                {
                    PlayerArray[0].SetPositionClientRpc(RedTeamSpawnTransform.position);
                    PlayerArray[1].SetPositionClientRpc(BlueTeamSpawnTransform.position);

                    PlayerArray[0].StartClientRpc();
                    PlayerArray[1].StartClientRpc();

                    PlayerArray[0].SetIsGameNowClientRpc(true);
                    PlayerArray[1].SetIsGameNowClientRpc(true);
                    isGameNow = true;
                }

            }
        }

        //Debug.Log("�ԃ`�[���̓��_�@�F" + RedTeamCount);
        //Debug.Log("�`�[���̓��_�@�F" + BlueTeamCount);
    }

    public void PlayerSet(PlayerController player, Transform tran)
    {
        if (PlayerArray[0] == null)
        {
            PlayerArray[0] = player;
            //tran.position = new Vector3(0.0f, 1.6f, -4.5f);
            //PlayerArray[0].SetPositionClientRpc(new Vector3(0.0f, 0.4f, -4.5f));
            PlayerArray[0].SetTeamClientRpc(PlayerController.Team.RedTeam);
        }
        else
        {
            PlayerArray[1] = player;
            //tran.position = new Vector3(0, 1.6f, 0);
            //PlayerArray[1].SetPositionClientRpc(new Vector3(0.0f, 0.4f, 0.0f));
            PlayerArray[1].SetTeamClientRpc(PlayerController.Team.BlueTeam);
        }
    }

    public void PlayerSetHP(PlayerController player,int nowHP)
    {
        /*
        if (player == PlayerArray[0])
            Player1HP = nowHP;
        else if (player == PlayerArray[1])
            Player2HP = nowHP;
        */

        if(player == PlayerArray[0] || player == PlayerArray[1])
        {
            player.SetNowHpClientRpc(nowHP);
            Debug.Log(player.name + "��HP�� " + nowHP + " �ł�");

            /*
            if(nowHP <= 0)
            {
                isGameEnd = true;
                //HP���Ȃ��Ȃ������̏���
                for(int i = 0;i<PlayerArray.Length;i++)
                {
                    PlayerArray[i].PlayerGameEndClientRpc();
                }
            }
            */
        }
    }

    public Sprite GetPlayer1Sprite()
    {
        Sprite ret = playerSpriteData.player1;
        return ret;
    }

    public Sprite GetPlayer2Sprite()
    {
        Sprite ret = playerSpriteData.player2;
        return ret;
    }

    public void SetAllPlayerInitLocation()
    {
        PlayerArray[0].SetPositionClientRpc(RedTeamSpawnTransform.position);
        PlayerArray[1].SetPositionClientRpc(BlueTeamSpawnTransform.position);
    }

    public void SetPlayerDead(PlayerController player)
    {
        for(int i = 0;i<PlayerArray.Length;i++)
        {
            if (PlayerArray[i] == player && !PlayerDeadFlg[i])
            {
                Debug.Log(player.name + "�̕����ł��B");
                PlayerDeadFlg[i] = true;
            }
        }
    }

    public void RestartMessage(PlayerController player)
    {
        for(int i = 0;i< PlayerArray.Length;i++)
        {
            if (PlayerArray[i] == player)
            {
                PlayerRestartInput[i] = true;
                Debug.Log(player.name + "�̃��X�^�[�g���󂯕t���܂���");
            }
        }
    }

    public void AddTeamCount(PlayerController.Team team,int addCount)
    {
        switch(team)
        {
            case PlayerController.Team.RedTeam:
                RedTeamCount += addCount;
                break;

            case PlayerController.Team.BlueTeam:
                BlueTeamCount += addCount;
                break;

            default:
                break;
        }

        SetTeamCountClientRpc(RedTeamCount, BlueTeamCount);
    }

    public Vector3 GetLobbyPosition()
    {
        return LobbyTransform.position;
    }

    [Unity.Netcode.ClientRpc]
    public void SetTeamCountClientRpc(int redCount,int blueCount)
    {
        RedTeamCount = redCount;
        BlueTeamCount = blueCount;
    }
}
