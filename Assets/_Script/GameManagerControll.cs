using System.Collections;
using System.Collections.Generic;
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

    public PlayerController player1 { get; private set; }
    public PlayerController player2 { get; private set; }

    public int Player1HP { get; private set; }
    public int Player2HP { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            GameObject.Destroy(this);
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
        }


    }

    public void PlayerSet(PlayerController player, Transform tran)
    {
        if (player1 == null)
        {
            player1 = player;
            //tran.position = new Vector3(0.0f, 1.6f, -4.5f);
            player1.SetInitPositionClientRpc(new Vector3(0.0f, 0.4f, -4.5f));
            player1.SetTeamClientRpc(PlayerController.Team.RedTeam);
        }
        else
        {
            player2 = player;
            //tran.position = new Vector3(0, 1.6f, 0);
            player2.SetInitPositionClientRpc(new Vector3(0.0f, 0.4f, 0.0f));
            player2.SetTeamClientRpc(PlayerController.Team.BlueTeam);
        }
    }

    [Unity.Netcode.ClientRpc]
    public void SetPlayerClientRpc(PlayerController player1,PlayerController player2)
    {

    }

    public void PlayerSetHP(PlayerController player,int nowHP)
    {
        if (player == player1)
            Player1HP = nowHP;
        else if (player == player2)
            Player2HP = nowHP;

        player.SetNowHpClientRpc(nowHP);
        Debug.Log(player.name + "のHPは " + nowHP + " です");
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
        player1.SetInitPositionClientRpc(new Vector3(0.0f, 0.4f, -4.5f));
        player2.SetInitPositionClientRpc(new Vector3(0.0f, 0.4f, 0.0f));
    }
}
