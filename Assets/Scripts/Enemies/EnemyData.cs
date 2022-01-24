using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Game Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Stats")]
    [Tooltip("How much damage it takes to kill this enemy")]
    [Range(1, 10000)]
    [SerializeField]
    private int health = 100;

    public int Health
    {
        get => health;
    }

    [Tooltip("How much damage this enemies attack deals")]
    [Range(1, 100)]
    [SerializeField]
    private int damage = 10;

    public int Damage
    {
        get => damage;
    }
}
