using System;
using Unity.Netcode;
using UnityEngine;

namespace VSPoliceReBoot.Player
{
    public class PlayerInput : NetworkBehaviour
    {
        [SerializeField] PlayerInputSO _playerInputSO;

        private void Awake()
        {
            _playerInputSO.PlayerInputInfo = new PlayerInputInfo();
        }
    }
}