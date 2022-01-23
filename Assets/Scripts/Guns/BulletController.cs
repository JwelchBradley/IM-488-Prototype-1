/*****************************************************************************
// File Name :         BulletController.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the movement and collisions of bullets.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Visuals")]
    [Tooltip("The decal spawned against the location this hits")]
    [SerializeField]
    private GameObject bulletDecal;

    [Tooltip("How long before the decal is despawned")]
    [Range(0.0f, 20.0f)]
    [SerializeField]
    private float decalEffectLifetime = 5.0f;

    [Tooltip("How long before the decal starts to fade out")]
    [Range(0.0f, 20.0f)]
    [SerializeField]
    private float decalTimeBeforeFadeOut = 2.0f;

    [Tooltip("The partical effect spawned at the collision point")]
    [SerializeField]
    private GameObject hitEffect;

    [Tooltip("How long before the hit effect is despawned")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float hitEffectLifetime = 1.0f;

    [Tooltip("Enter the name of the material and the audioclip associated with it")]
    [SerializeField]
    private Dictionary<string, AudioClip> hitSound = new Dictionary<string, AudioClip>();

    [Header("Stats")]
    [Tooltip("How fast the bullet moves before self destructing")]
    [SerializeField]
    private float timeToDestroy = 5.0f;

    /// <summary>
    /// Holds the amount of damage this bullet deals.
    /// </summary>
    protected int damage = 0;

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
    protected float speed = 0.0f;

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

    /// <summary>
    /// Calls for the collision event to happen.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        CollisionEvent(other);
    }

    /// <summary>
    /// Handles what ever is supposed to happen during this bullets collision.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void CollisionEvent(Collision other)
    {
        ContactPoint contact = other.GetContact(0);
        GameObject decal = Instantiate(bulletDecal, contact.point + contact.normal * 0.01f, Quaternion.LookRotation(contact.normal));
        decal.transform.parent = other.gameObject.transform;
        StartCoroutine(decal.GetComponent<DecalBehaviour>().FadeOut(decalEffectLifetime, decalTimeBeforeFadeOut));
        GameObject particalEffect = Instantiate(hitEffect, contact.point + contact.normal * 0.01f, Quaternion.LookRotation(contact.normal));

        Destroy(decal, decalEffectLifetime);
        Destroy(particalEffect, hitEffectLifetime);

        string matKey = other.gameObject.GetComponent<Renderer>().material.name;
        // Plays hitSound based off of the targets material
        //AudioClip sound = hitSound["Default"];
        //hitSound.TryGetValue(matKey, out sound);
        //AudioSource.PlayClipAtPoint(sound, contact.point);

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.UpdateHealth(-damage);
        }

        Destroy(gameObject);
    }
}
