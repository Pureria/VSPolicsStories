using System;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.Serialization;
using VSPoliceReBoot.Network;
using VSPoliceReBoot.Object;

namespace VSPoliceReBoot.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkObjectBase, ICivObject
    {
        [SerializeField] private PlayerInputSO _playerInputSO;
        [SerializeField] private PlayerStatesSO _playerStatesSO;
        [SerializeField] private GameObject _playerCamera;
        [SerializeField]private SpriteRenderer _playerSpriteRend;
        private bool _canMove;
        private Rigidbody _rb;

        #region Network Callbacks
        public override void OnNetworkStart()
        {
            _rb = GetComponent<Rigidbody>();
            _canMove = true;
            CinemachineVirtualCamera camera = _playerCamera.GetComponent<CinemachineVirtualCamera>();
            camera.Follow = transform;
        }
        #endregion

        #region Owner Callbacks
        
        public override void OnOwnerStart()
        {
            //プレイヤーカメラ生成
            var playerCamera = Instantiate(_playerCamera, transform.position, _playerCamera.transform.rotation);
            playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;
            _playerSpriteRend.enabled = true;
            Debug.Log("オーナー接続");
        }
        
        public override void OnOwnerPreUpdate()
        {
            SendInputServerRpc(_playerInputSO.PlayerInputInfo);
            CheckCivObject();
        }
        #endregion

        #region Non Owner Callbacks
        public override void OnNonOwnerStart()
        {
            _playerSpriteRend.enabled = false;
            Debug.Log("オーナー以外接続");
        }
        #endregion

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
            Rot(inputInfo.ViewPoint);
        }

        /// <summary>
        /// プレイヤーの移動処理
        /// </summary>
        /// <param name="moveInput"></param>
        private void Move(Vector2 moveInput)
        {
            if (!_canMove) return;
            
            //プレイヤーの移動処理
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
            move = move * _playerStatesSO.MoveSpeed;
            _rb.velocity = move;
        }

        /// <summary>
        /// プレイヤーの向きを変更
        /// </summary>
        /// <param name="rotDirection"></param>
        private void Rot(Vector3 rotDirection)
        {
            //プレイヤーの向きを変更
            rotDirection = rotDirection - transform.position;
            rotDirection.y = 0;
            transform.forward = rotDirection;
        }

        /// <summary>
        /// CIVObjectが視界に入っているか確認
        /// </summary>
        private void CheckCivObject()
        {
            //CIVObjectが視界に入った場合EnterCIVを呼び出す。視界はプレイヤーの前方からViewAngleで指定した角度の範囲
            Collider[] colliders = Physics.OverlapSphere(transform.position, _playerStatesSO.ViewDistance);
            foreach (var collider in colliders)
            {
                //自分自身の場合はスキップ
                if(collider.transform == transform) continue;
                //プレイヤーの前方からCIVObjectまでの方向を取得
                Vector3 dir = collider.transform.position - transform.position;
                float angle = Vector3.Angle(dir, transform.forward);
                if (angle <= _playerStatesSO.ViewAngle)
                {
                    //CIVObjectのインターフェースを取得してEnterCIVを呼び出す
                    ICivObject civObject = collider.GetComponent<ICivObject>();
                    civObject?.OnEnterCIV();
                }
            }
        }

        public void OnEnterCIV()
        {
            //if (IsOwner) return;
            _playerSpriteRend.enabled = true;
        }

        public void OnExitCIV()
        {
            //if (IsOwner) return;
            _playerSpriteRend.enabled = false;
        }
    }

    /// <summary>
    /// プレイヤーの入力情報
    /// </summary>
    [Serializable]
    public class PlayerInputInfo : INetworkSerializable
    {
        public Vector2 MoveInput;
        public Vector3 ViewPoint;
        public bool ShotInput;
        public bool UseShotInput;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref MoveInput);
            serializer.SerializeValue(ref ShotInput);
            serializer.SerializeValue(ref UseShotInput);
            serializer.SerializeValue(ref ViewPoint);
        }
    }
}
