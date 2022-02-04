/*****************************************************************************
// File Name :         ThirdPersonController.cs
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
    private AudioSource aSource;
    public AudioClip destroy;
    private Renderer render;
    private int health = 30;
    private float velocityThreshold = 30.0f*30.0f;

    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        aSource = GetComponent<AudioSource>();
        aSource.clip = destroy;
        tpc = GameObject.Find("Player").GetComponent<ThirdPersonController>();
        rb = GetComponent<Rigidbody>();

    }

    private void OnCollisionEnter(Collision collision)
    {
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
            
            Destroy(collision.gameObject, 0.2f);
        }


    }

    public void UpdateHealth(int healthMod)
    {
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
        aSource.Play();
        Instantiate(particle, gameObject.transform.position, Quaternion.identity);
        render.enabled = false;
        Destroy(gameObject, 0.2f);
    }

   
}
