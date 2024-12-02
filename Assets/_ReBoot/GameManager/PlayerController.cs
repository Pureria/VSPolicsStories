using System;
using _ReBoot;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace VSPoliceReBoot.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour, ICivObject
    {
        [SerializeField] private PlayerInputSO _playerInputSO;
        [SerializeField] private PlayerStatesSO _playerStatesSO;
        [SerializeField] private GameObject _playerCamera;
        private bool _canMove;
        private SpriteRenderer _playerSpriteRend;
        private Rigidbody _rb;

        private void Awake()
        {
            if (!IsOwner) _playerSpriteRend.enabled = false;
            
            _rb = GetComponent<Rigidbody>();
            _canMove = true;
            CinemachineVirtualCamera camera = _playerCamera.GetComponent<CinemachineVirtualCamera>();
            camera.Follow = transform;
        }

        private void Update()
        {
            //オーナーのみ処理を行う
            if (!IsOwner)
                return; 
            
            SendInputServerRpc(_playerInputSO.PlayerInputInfo);
            CheckCivObject();
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
            Rot(inputInfo.RotDirection);
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

        private void CheckCivObject()
        {
            //CIVObjectが視界に入った場合EnterCIVを呼び出す。視界はプレイヤーの前方からViewAngleで指定した角度の範囲
            Collider[] colliders = Physics.OverlapSphere(transform.position, _playerStatesSO.ViewDistance);
            foreach (var collider in colliders)
            {
                Vector3 dir = collider.transform.position - transform.position;
                float angle = Vector3.Angle(dir, transform.forward);
                if (angle <= _playerStatesSO.ViewAngle)
                {
                    ICivObject civObject = collider.GetComponent<ICivObject>();
                    civObject?.OnEnterCIV();
                }
            }
        }

        public void OnEnterCIV()
        {
            if (IsOwner) return;
            _playerSpriteRend.enabled = true;
        }

        public void OnExitCIV()
        {
            if (IsOwner) return;
            _playerSpriteRend.enabled = false;
        }
    }

    [Serializable]
    public class PlayerInputInfo : INetworkSerializable
    {
        public Vector2 MoveInput;
        public Vector2 RotDirection;
        public bool ShotInput;
        public bool UseShotInput;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref MoveInput);
            serializer.SerializeValue(ref ShotInput);
            serializer.SerializeValue(ref UseShotInput);
            serializer.SerializeValue(ref RotDirection);
        }
    }
}
