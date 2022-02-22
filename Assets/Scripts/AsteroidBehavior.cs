/*****************************************************************************
// File Name :         AsteroidBehavior.cs
// Author :            Jessica Barthelt
// Creation Date :     1 February 2022
//
// Brief Description : Player loses health every time they hit an asteroid
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AsteroidBehavior : MonoBehaviour, IDamagable
{

    ThirdPersonController tpc;

    IDamagable damagable;

    Rigidbody rb;
    private Renderer render;
    private Collider col;
    private int health = 30;
    private float velocityThreshold = 30.0f*30.0f;

    public GameObject particle;

    [HideInInspector]
    public bool currentlyHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponentInChildren<Renderer>();
        tpc = GameObject.Find("Player").GetComponent<ThirdPersonController>();
        rb = GetComponent<Rigidbody>();
        col = GetComponentInChildren<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentlyHeld && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet")))
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            tpc.colImage.SetActive(true);
            tpc.colAsteroid = true;
            AsteroidDestruction();
            tpc.UpdateHealth(-5);
        }
        else if (collision.relativeVelocity.sqrMagnitude > velocityThreshold && collision.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.UpdateHealth(-100);
            UpdateHealth(-100);
        }
        else if(collision.relativeVelocity.sqrMagnitude > velocityThreshold && !collision.gameObject.CompareTag("Bullet"))
        {
            UpdateHealth(-100);
        }
    }

    public void UpdateHealth(int healthMod)
    {
        if (currentlyHeld && healthMod > -50)
            return;

        health += healthMod;

        if(health <= 0 && render.enabled)
        {
            AsteroidDestruction();
        }
    }

    public int HealthAmount()
    {
        throw new System.NotImplementedException();
    }

    private void AsteroidDestruction()
    {
        Instantiate(particle, gameObject.transform.position, Quaternion.identity);
        render.enabled = false;
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            // objectA is not the attached GameObject, so you can do all your checks with it.
            var objectA = gameObject.transform.GetChild(i);

            objectA.transform.parent = null;
            objectA.gameObject.SetActive(false);
        }

        col.enabled = false;
        Destroy(gameObject, 0.2f);
    }
}
