using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidBehavior : MonoBehaviour
{

    ThirdPersonController tpc;
    Behavior beh;

    // Start is called before the first frame update
    void Start()
    {
        tpc = GameObject.Find("Player").GetComponent<ThirdPersonController>();
        beh = GameObject.Find("Player").GetComponent<Behavior>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("hit player");
            tpc.UpdateHealth(-5);
            
            Destroy(this.gameObject);
        }
    }
}
