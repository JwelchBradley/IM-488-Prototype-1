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
    private GunData gunData;

    public GunData GunData
    {
        set
        {
            gunData = value;
        }
    }

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
        Invoke("DestroyTimer", 0.2f);
    }

    private void DestroyTimer()
    {
        Destroy(gameObject, gunData.TimeToDestroy);
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
        GameObject decal = Instantiate(gunData.BulletDecal, contact.point + contact.normal * 0.01f, Quaternion.LookRotation(contact.normal));
        decal.transform.parent = other.gameObject.transform;
        decal.GetComponent<DecalBehaviour>().StartFadeOut(gunData.DecalEffectLifetime, gunData.DecalTimeBeforeFadeOut);
        GameObject particalEffect = Instantiate(gunData.HitEffect, contact.point + contact.normal * 0.01f, Quaternion.LookRotation(contact.normal));

        Destroy(decal, gunData.DecalEffectLifetime);
        Destroy(particalEffect, gunData.HitEffectLifetime);

        //string matKey = other.gameObject.GetComponent<Renderer>().material.name;
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
