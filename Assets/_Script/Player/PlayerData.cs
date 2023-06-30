using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player HP")]
    public float maxHP = 100.0f;

    [Header("Move State")]
    public float moveSpeed = 5.0f;
    public float runSpeed = 8.0f;

    [Header("Shot Move State")]
    public float shotMoveSpeed = 1.5f;
    public float reloadMoveSpeed = 1.5f;

    [Header("Melee")]
    public float meleeDamage = 50.0f;
    public float meleeDistance = 5.0f;

    [Header("Interact Layer")]
    public string interactLayerName;
    public float interactDistance = 5.0f;
}
