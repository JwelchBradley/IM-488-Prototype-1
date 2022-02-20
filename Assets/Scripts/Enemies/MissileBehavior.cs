using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour, IDamagable
{
    IDamagable damagable;

    private int health = 5;

    public Transform target;

    public float speed = 1f;
    ThirdPersonController tpc;

    // Start is called before the first frame update
    void Start()
    {
        tpc = GameObject.Find("Player").GetComponent<ThirdPersonController>();
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    public void UpdateHealth(int healthMod)
    {
        health += healthMod;

        if (health <= 0)
        {
            EnemyDestruction();
        }
    }

    public int HealthAmount()
    {
        throw new System.NotImplementedException();
    }

    private void EnemyDestruction()
    {

        Destroy(gameObject, 0.2f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            tpc.UpdateHealth(-20);
            Destroy(gameObject, 0.2f);

        }
    }
}
