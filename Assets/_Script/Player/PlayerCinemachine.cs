using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCinemachine : NetworkBehaviour
{
    private enum playerNumber
    {
        player1,
        player2,
    }

    private CinemachineVirtualCamera _camera;

    private void Start()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (_camera.Follow == null)
        {
            if (PlayerController.Owner != null)
                _camera.Follow = PlayerController.Owner.transform;
        }
    }
}
