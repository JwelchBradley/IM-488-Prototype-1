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

public class AsteroidBehavior : MonoBehaviour
{

    ThirdPersonController tpc;

    IDamagable damagable;

    Rigidbody rb;
    public AudioSource aSource;
    public AudioClip destroy;

    // Start is called before the first frame update
    void Start()
    {
        tpc = GameObject.Find("Player").GetComponent<ThirdPersonController>();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            aSource.clip = destroy;
            aSource.Play();
            Destroy(this.gameObject, 0.2f);
            print("hit player");
            tpc.health -= 5;
            tpc.UpdateHealthBar();
            
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
