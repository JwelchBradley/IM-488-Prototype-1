using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamagable
{
    /// <summary>
    /// The current health total of this enemy.
    /// </summary>
    private int currentHealth = 0;

    [SerializeField]
    protected EnemyData enemyData;

    [SerializeField]
    private GameObject onLight;

    private void OnEnable()
    {
        if(onLight != null)
        onLight.SetActive(true);
    }

    private void OnDisable()
    {
        if(onLight != null)
        onLight.SetActive(false);
    }

    /// <summary>
    /// Holds a reference to the player usually used for attack functions.
    /// </summary>
    protected Transform playerReference;

    protected virtual void Awake()
    {
        currentHealth = enemyData.Health;
        playerReference = GameObject.Find("Player").transform.Find("Pivot");
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
        gameObject.SetActive(false);
    }

    public int HealthAmount()
    {
        throw new System.NotImplementedException();
    }
}
