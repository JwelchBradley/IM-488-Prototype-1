using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemyBehavior : MonoBehaviour, IDamagable
{
    IDamagable damagable;

    private Renderer render;
    private int health = 30;
    public bool playerDetected;

    // Start is called before the first frame update
    void Start()
    {
        playerDetected = false;
    }

    public void UpdateHealth(int healthMod)
    {
        health += healthMod;

        if (health <= 0)
        {
            EnemyDestruction();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDetected == true)
        {
            print("player in range");
        }
    }

    public int HealthAmount()
    {
        throw new System.NotImplementedException();
    }

    private void EnemyDestruction()
    {

        Destroy(gameObject, 0.2f);
    }
}
