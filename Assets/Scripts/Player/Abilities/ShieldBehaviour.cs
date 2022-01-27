using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour, IDamagable
{
    private int health;

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

    public void Initialization(float duration, int health)
    {
        this.health = health;
        DestroyShield(duration);
    }

    private void DestroyShield(float endTime)
    {
        Destroy(gameObject, endTime);
    }
    #endregion
}
