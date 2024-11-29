using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace VSPoliceReBoot.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float _viewAngle = 45.0f;
        [SerializeField] private PlayerInputSO _playerInputSO;
        private SpriteRenderer _playerSpriteRend;

        private void Update()
        {
            //オーナーのみ処理を行う
            if (!IsOwner)
                return; 
            
            SendInputServerRpc();
        }

        /// <summary>
        /// プレイヤーの入力をサーバーに送信
        /// </summary>
        [ServerRpc]
        private void SendInputServerRpc()
        {
        }

        /// <summary>
        /// クライアントからの入力を受け取る
        /// </summary>
        private void HandleClientInput()
        {
        }
    }
}
