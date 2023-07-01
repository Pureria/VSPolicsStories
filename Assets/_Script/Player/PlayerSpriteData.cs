using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerSpriteData", menuName = "Data/Player Data/Sprite Data")]
public class PlayerSpriteData : ScriptableObject
{
    [Header("Player Sprite")]
    public Sprite player1;
    public Sprite player2;
}
