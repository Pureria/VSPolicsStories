using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManagerControll : NetworkBehaviour
{
    public static GameManagerControll Singleton;
    public enum playerNumber
    {
        player1,
        player2,
    }

    [SerializeField]
    private PlayerSpriteData playerSpriteData;

    public PlayerController[] PlayerArray { get; private set; } = new PlayerController[2];
    private bool[] PlayerDeadFlg;
    private bool[] PlayerRestartInput;
    public bool isGameEnd { get; private set; }


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
        //サーバーの時以外処理しない
        if (!this.IsServer)
        {
            //サーバーじゃないときの処理
        }
        else
        {
            //サーバーの時の処理
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
                    //TODO::リスタート処理
                    Debug.Log("全員がリスタートを選択しました");
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
                }
            }
            else
            {
                for(int i = 0;i<PlayerDeadFlg.Length;i++)
                {
                    if (PlayerDeadFlg[i])
                        isGameEnd = true;
                }
            }
        }


    }

    public void PlayerSet(PlayerController player, Transform tran)
    {
        if (PlayerArray[0] == null)
        {
            PlayerArray[0] = player;
            //tran.position = new Vector3(0.0f, 1.6f, -4.5f);
            PlayerArray[0].SetInitPositionClientRpc(new Vector3(0.0f, 0.4f, -4.5f));
            PlayerArray[0].SetTeamClientRpc(PlayerController.Team.RedTeam);
        }
        else
        {
            PlayerArray[1] = player;
            //tran.position = new Vector3(0, 1.6f, 0);
            PlayerArray[1].SetInitPositionClientRpc(new Vector3(0.0f, 0.4f, 0.0f));
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
            Debug.Log(player.name + "のHPは " + nowHP + " です");

            /*
            if(nowHP <= 0)
            {
                isGameEnd = true;
                //HPがなくなった時の処理
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
        PlayerArray[0].SetInitPositionClientRpc(new Vector3(0.0f, 0.4f, -4.5f));
        PlayerArray[1].SetInitPositionClientRpc(new Vector3(0.0f, 0.4f, 0.0f));
    }

    public void SetPlayerDead(PlayerController player)
    {
        for(int i = 0;i<PlayerArray.Length;i++)
        {
            if (PlayerArray[i] == player && !PlayerDeadFlg[i])
            {
                Debug.Log(player.name + "の負けです。");
                PlayerDeadFlg[i] = true;
                //HPがなくなった時の処理
                for (int j = 0; j < PlayerArray.Length; j++)
                {
                    PlayerArray[j].PlayerGameEndClientRpc();
                }
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
                Debug.Log(player.name + "のリスタートを受け付けました");
            }
        }
    }
}
