using UnityEngine;

namespace VSPoliceReBoot.Player
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ReBoot/Player/Input")]
    public class PlayerInputSO : ScriptableObject
    {
        public Vector2 MoveInput;
        public bool ShotInput;
        public bool UseShotInput;
    }
}