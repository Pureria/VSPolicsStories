using UnityEngine;
using UnityEngine.Serialization;

namespace VSPoliceReBoot.Player
{
    [CreateAssetMenu(fileName = "PlayerStatesSO", menuName = "ReBoot/Player/States")]
    public class PlayerStatesSO : ScriptableObject
    {
        public float MoveSpeed = 5.0f;
        public float ViewAngle = 45.0f;
        public float ViewDistance = 10.0f;
    }
}