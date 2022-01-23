/*****************************************************************************
// File Name :         BulletController.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the movement and collisions of bullets.
*****************************************************************************/
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Tooltip("The decal spawned against the location this hits")]
    [SerializeField]
    private GameObject bulletDecal;

    [Tooltip("How fast the bullet moves before self destructing")]
    [SerializeField]
    private float timeToDestroy = 5.0f;

    /// <summary>
    /// Holds the amount of damage this bullet deals.
    /// </summary>
    private int damage = 0;

    /// <summary>
    /// Holds the amount of damage this bullet deals.
    /// </summary>
    public int Damage
    {
        set
        {
            damage = value;
        }
    }

    /// <summary>
    /// Holds how fast the bullet travels.
    /// </summary>
    private float speed = 0.0f;

    /// <summary>
    /// Holds how fast the bullet travels.
    /// </summary>
    public float Speed
    {
        set
        {
            speed = value;
        }
    }

    /// <summary>
    /// The target of this bullet.
    /// </summary>
    private Vector3 target;

    /// <summary>
    /// The target of this bullet.
    /// </summary>
    public Vector3 Target
    {
        get => target;

        set
        {
            target = value;
        }
    }

    /// <summary>
    /// Sets the bullet to be destroyed after a specified amount of time.
    /// </summary>
    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    /// <summary>
    /// Moves the bullet to its target position.
    /// </summary>
    private void Update()
    {
        MoveBullet();
    }

    /// <summary>
    /// Moves the bullet to its target position.
    /// </summary>
    private void MoveBullet()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);
        Instantiate(bulletDecal, contact.point + contact.normal * 0.01f, Quaternion.LookRotation(contact.normal));

        ThirdPersonController tpc = GetComponent<ThirdPersonController>();

        if(tpc != null)
        {
            tpc.UpdateHealth(-damage);
        }

        Destroy(gameObject);
    }
}
