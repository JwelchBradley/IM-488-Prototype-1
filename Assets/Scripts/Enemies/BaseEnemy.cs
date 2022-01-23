using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    [Tooltip("How much damage it takes to kill this enemy")]
    [Range(1, 10000)]
    [SerializeField]
    protected int health = 100;

    /// <summary>
    /// The current health total of this enemy.
    /// </summary>
    private int currentHealth = 0;

    [Tooltip("How much damage this enemies attack deals")]
    [Range(1, 100)]
    [SerializeField]
    protected int damage = 10;

    /// <summary>
    /// Holds a reference to the player usually used for attack functions.
    /// </summary>
    protected GameObject playerReference;

    private void Awake()
    {
        currentHealth = health;
        playerReference = GameObject.Find("Player");
    }

    /// <summary>
    /// Allows the players health to be increase/decreased.
    /// </summary>
    /// <param name="healthMod">Amount of health added to the players current total. (If damage use negative number)</param>
    public void UpdateHealth(int healthMod)
    {
        health += healthMod;

        if (currentHealth <= 0)
        {
            EnemyDeath();
        }
        else if (currentHealth > health)
        {
            currentHealth = health;
        }
    }

    protected virtual void Attack()
    {
        
    }

    protected virtual void EnemyDeath()
    {
        Destroy(gameObject);
    }
}
