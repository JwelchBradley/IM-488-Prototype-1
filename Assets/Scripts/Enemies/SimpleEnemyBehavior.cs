/*****************************************************************************
// File Name :         SimpleEnemyBehavior.cs
// Author :            Jessica Barthelt
// Creation Date :     9 February 2022
//
// Brief Description : Enemy takes damage when hit by player bullets
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyBehavior : MonoBehaviour, IDamagable
{
    IDamagable damagable;

    private Renderer render;
    private int health = 30;

    public float speed = 6f;

    public Transform target;

    public bool shooting;

    float distance;

    // Start is called before the first frame update
    void Start()
    {
        shooting = false;
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(target.transform.position, transform.position);

        if (shooting == false)
        {
            
            if (distance <= 30)
            {
                print("player in range");
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            }
            else
            {
                speed = 0;
            }
        }
        else
        {
            speed = 0;
        }
        
        
    }

    public void UpdateHealth(int healthMod)
    {
        health += healthMod;

        if (health <= 0)
        {
            EnemyDestruction();
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
