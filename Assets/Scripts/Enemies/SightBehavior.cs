using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightBehavior : MonoBehaviour
{

    DetectEnemyBehavior deb;

    // Start is called before the first frame update
    void Start()
    {
        deb = FindObjectOfType<DetectEnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            deb.playerDetected = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            deb.playerDetected = false;

        }
    }

}
