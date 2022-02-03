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
  

    // Start is called before the first frame update
    void Start()
    {
        tpc = GameObject.Find("Player").GetComponent<ThirdPersonController>();
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            print("hit player");
            tpc.health -= 5;
            tpc.UpdateHealthBar();
            
        }
    }
}
