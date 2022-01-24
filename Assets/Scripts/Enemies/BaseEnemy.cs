using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamagable
{
    /// <summary>
    /// The current health total of this enemy.
    /// </summary>
    private int currentHealth = 0;

    [SerializeField]
    private EnemyData enemyData;

    /// <summary>
    /// Holds a reference to the player usually used for attack functions.
    /// </summary>
    protected GameObject playerReference;

    private void Awake()
    {
        currentHealth = enemyData.Health;
        playerReference = GameObject.Find("Player");
    }

    /// <summary>
    /// Allows the players health to be increase/decreased.
    /// </summary>
    /// <param name="healthMod">Amount of health added to the players current total. (If damage use negative number)</param>
    public void UpdateHealth(int healthMod)
    {
        currentHealth += healthMod;

        if (currentHealth <= 0)
        {
            EnemyDeath();
        }
        else if (currentHealth > enemyData.Health)
        {
            currentHealth = enemyData.Health;
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
