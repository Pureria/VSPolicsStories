using System;
using Unity.Netcode;
using UnityEngine;

namespace _ReBoot.GameManager
{
    [Serializable]
    public class PlayerInfo : INetworkSerializable
    {
        private int _playerId;
        private string _playerName;
        private int _playerScore;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _playerId);
            serializer.SerializeValue(ref _playerName);
            serializer.SerializeValue(ref _playerScore);
        }
    }
}