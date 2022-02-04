using MyBox;
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

    [Header("Specific Values")]
    [Tooltip("The type of ability this is")]
    public type enemyType = type.shooting;

    public enum type
    {
        shooting,
        melee
    }

    #region Specifics
    [Header("Specifics")]


    [ConditionalField("enemyType", type.shooting)]
    [Tooltip("How fast the enemy shoots bullets")]
    [SerializeField] private GunData gunData;

    public GunData GunData
    {
        get => gunData;
    }

    [ConditionalField("enemyType", type.shooting)]
    [Tooltip("How far away the turrent will shoot at the player from")]
    [SerializeField] private float range = 50.0f;

    public float Range
    {
        get => range;
    }
    #endregion
}
