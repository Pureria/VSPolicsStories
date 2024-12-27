using UnityEngine;

namespace VSPoliceReBoot.Player
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ReBoot/Player/Input")]
    public class PlayerInputSO : ScriptableObject
    {
        public PlayerInputInfo PlayerInputInfo;
    }
}