using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VSPoliceReBoot.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] PlayerInputSO _playerInputSO;

        private void Awake()
        {
            _playerInputSO.PlayerInputInfo = new PlayerInputInfo();
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            var rawMovementInput = context.ReadValue<Vector3>();
            _playerInputSO.PlayerInputInfo.MoveInput.x = Mathf.RoundToInt(rawMovementInput.x);
            _playerInputSO.PlayerInputInfo.MoveInput.y = Mathf.RoundToInt(rawMovementInput.z);
        }
        
        public void OnMousePosition(InputAction.CallbackContext context)
        {
            Vector3 vec = context.ReadValue<Vector2>();
            vec.z = 10.0f;
            vec = Camera.main.ScreenToWorldPoint(vec);
            vec.y = 0;
            _playerInputSO.PlayerInputInfo.ViewPoint = vec;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_playerInputSO.PlayerInputInfo.ViewPoint, 0.5f);
        }
    }
}