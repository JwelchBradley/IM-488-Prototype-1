using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour, IDamagable
{
    IDamagable damagable;

    private int health = 5;

    public Transform target;
    private Renderer render;
    public float speed = 3f;
    ThirdPersonController tpc;

    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponentInChildren<Renderer>();
        tpc = GameObject.Find("Player").GetComponent<ThirdPersonController>();
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector3 targetDirection = target.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
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

        Destroy(gameObject, 0.2f);
    }
}
