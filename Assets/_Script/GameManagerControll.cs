using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManagerControll : NetworkBehaviour
{
    public static GameManagerControll Singleton;

    public PlayerController player1 { get; private set; }
    public PlayerController player2 { get; private set; }

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
            return;


    }

    public void PlayerSet(PlayerController player, Transform tran)
    {
        if (player1 == null)
        {
            player1 = player;
            tran.position = new Vector3(0.0f, 0.2f, -1.5f);
        }
        else
        {
            player2 = player;
            tran.position = new Vector3(0, 0.2f, 0);
        }
    }
}
