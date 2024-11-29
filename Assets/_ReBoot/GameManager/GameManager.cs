using System;
using Unity.Netcode;
using UnityEngine;

namespace _ReBoot.GameManager
{
    
    public class GameManager : NetworkBehaviour
    {
        public const int MaxPlayer = 2;
        private bool _isGame;

        private void Update()
        {
            if (!IsServer) return;

            if (_isGame) GameUpdate();
            else StandByGameUpdate();
        }

        private void StandByGameUpdate()
        {

        }

        private void GameUpdate()
        {

        }

        private void StartGame()
        {
            _isGame = true;
        }

        private void EndGame()
        {
            _isGame = false;
        }
    }
}