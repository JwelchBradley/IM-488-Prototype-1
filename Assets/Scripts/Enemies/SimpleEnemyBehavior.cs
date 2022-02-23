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

public class SimpleEnemyBehavior : MonoBehaviour
{
    IDamagable damagable;

    private Renderer render;
    private int health = 30;

    public float speed = 6f;

    [SerializeField] private float followDist;

    public Transform target;

    public bool shooting;

    float distance;

    public AudioSource aSource;
    public AudioClip death;

    // Start is called before the first frame update
    void Start()
    {
        followDist *= followDist;
        shooting = false;
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = (target.transform.position - transform.position).sqrMagnitude;

        if (shooting == false)
        {
            
            if (distance <= followDist)
            {
                float step = speed * Time.fixedDeltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                transform.LookAt(target.position);
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
}
