using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyTeamBoarder : NetworkBehaviour
{
    public PlayerController player { get; private set; }
    public bool onPlayer { get; private set; }
    private bool onPlayerClient;

    [SerializeField]
    private Material onPlayerMaterial;
    private Material normalMaterial;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        player = null;
        onPlayer = false;
        onPlayerClient = false;
        meshRenderer = GetComponent<MeshRenderer>();
        normalMaterial = meshRenderer.material;
    }

    private void Update()
    {
        if(onPlayer && !onPlayerClient)
        {
            //プレイヤーが上にいるとき
            onPlayerClient = true;
            meshRenderer.material = onPlayerMaterial;
        }
        else if(!onPlayer && onPlayerClient)
        {
            //プレイヤーが上にいない場合
            onPlayerClient = false;
            meshRenderer.material = normalMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || player != null) return;

        player = other.GetComponent<PlayerController>();

        if(player　!= null)
        {
            onPlayer = true;
            //meshRenderer.material = onPlayerMaterial;
            SetOnPlayerClientRpc(true);
        }
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
                //meshRenderer.material = normalMaterial;
                SetOnPlayerClientRpc(false);
            }
        }
    }

    [Unity.Netcode.ClientRpc]
    private void SetOnPlayerClientRpc(bool flg)
    {
        onPlayer = flg;
    }
}
