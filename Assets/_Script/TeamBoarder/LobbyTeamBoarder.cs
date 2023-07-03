using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyTeamBoarder : NetworkBehaviour
{
    public PlayerController player { get; private set; }
    public bool onPlayer { get; private set; }

    private void Start()
    {
        player = null;
        onPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        player = other.GetComponent<PlayerController>();

        if(player!= null)
            onPlayer = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsServer) return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;

        if(player != null)
        {
            if (other.gameObject == player.gameObject)
            {
                player = null;
                onPlayer = false;
            }
        }
    }
}
