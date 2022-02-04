using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour, IDamagable
{
    private int health;

    private Transform pivot;

    public int Health
    {
        set
        {
            health = value;
        }
    }

    #region Functions
    public void UpdateHealth(int healthMod)
    {
        health += healthMod;

        if(health <= 0)
        {
            DestroyShield(0.0f);
        }
    }

    public void Initialization(float duration, int health, Transform pivot)
    {
        this.health = health;
        this.pivot = pivot;
        DestroyShield(duration);
    }

    private void Update()
    {
        // Can't parent it to the play because collision get detected as player and don't damage shield
        transform.position = pivot.position;
    }

    private void DestroyShield(float endTime)
    {
        Destroy(gameObject, endTime);
    }

    public int HealthAmount()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
