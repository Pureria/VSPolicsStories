using UnityEngine;
using UnityEngine.Serialization;

namespace VSPoliceReBoot.Player
{
    [CreateAssetMenu(fileName = "PlayerStatesSO", menuName = "ReBoot/Player/States")]
    public class PlayerStatesSO : ScriptableObject
    {
        public float MoveSpeed = 5.0f;
    }
}