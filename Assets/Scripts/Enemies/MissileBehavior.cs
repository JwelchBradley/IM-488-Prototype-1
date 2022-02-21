using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    

    private int health = 5;

    public Transform target;
    private Renderer render;
    public float speed = 10f;
    ThirdPersonController tpc;
    private Rigidbody rb;

    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponentInChildren<Renderer>();
        rb = GetComponent<Rigidbody>();
        transform.Rotate(00, 0, 0);
        tpc = GameObject.Find("Player").GetComponent<ThirdPersonController>();
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
       
        float step = speed * Time.deltaTime;
      
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        
    }

    

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            tpc.UpdateHealth(-20);
            MissileDestruction();

        }
        if (collision.gameObject.tag == "Player Bullet")
        {
            MissileDestruction();

        }
    }


    private void MissileDestruction()
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
        gameObject.SetActive(false);
        //Destroy(gameObject, 0.2f);
    }
}
