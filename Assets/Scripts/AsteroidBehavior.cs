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
public class AsteroidBehavior : MonoBehaviour
{

    ThirdPersonController tpc;

    IDamagable damagable;

    Rigidbody rb;
    private AudioSource aSource;
    public AudioClip destroy;
    private Renderer render;

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
            //AudioSource.PlayClipAtPoint(destroy, transform.position);
            aSource.Play();
            render.enabled = false;
            Destroy(gameObject, 0.2f);
            tpc.UpdateHealth(-5);
            //tpc.UpdateHealthBar();
            
        }
        if (collision.gameObject.tag == "asteroid")
        {
            //aSource.clip = destroy;
            //aSource.Play();
            Destroy(this.gameObject, 0.2f);
            Destroy(collision.gameObject, 0.2f);
        }


    }


}
