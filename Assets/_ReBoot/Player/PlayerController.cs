using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Object = System.Object;

namespace VSPoliceReBoot.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float _viewAngle = 45.0f;
        [SerializeField] private PlayerInputSO _playerInputSO;
        [SerializeField] private PlayerStatesSO _playerStatesSO;
        private bool _canMove;
        private SpriteRenderer _playerSpriteRend;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _canMove = true;
        }

        private void Update()
        {
            //オーナーのみ処理を行う
            if (!IsOwner)
                return; 
            
            SendInputServerRpc(_playerInputSO.PlayerInputInfo);
        }

        /// <summary>
        /// プレイヤーの入力をサーバーに送信
        /// </summary>
        [ServerRpc]
        private void SendInputServerRpc(PlayerInputInfo inputInfo)
        {
            HandleClientInput(inputInfo);
        }

        /// <summary>
        /// クライアントからの入力を受け取る
        /// </summary>
        private void HandleClientInput(PlayerInputInfo inputInfo)
        {
            //プレイヤーの入力による動き
            Move(inputInfo.MoveInput);
        }

        private void Move(Vector2 moveInput)
        {
            if (!_canMove) return;
            
            //プレイヤーの移動処理
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
            move = move * _playerStatesSO.MoveSpeed;
            _rb.velocity = move;
        }

        private void Rot(Vector2 rotDirection)
        {
            //プレイヤーの向きを変更
            Vector3 rot = new Vector3(0, rotDirection.x, 0);
            transform.Rotate(rot);
        }
    }

    [Serializable]
    public class PlayerInputInfo
    {
        public Vector2 MoveInput;
        public bool ShotInput;
        public bool UseShotInput;
    }
}
