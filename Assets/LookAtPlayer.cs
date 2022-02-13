using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform.Find("Pivot");
    }

    private void FixedUpdate()
    {
        transform.LookAt(player);
    }
}
