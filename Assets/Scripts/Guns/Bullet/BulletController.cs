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
    GunData gunData;
    /*
    /// <summary>
    /// Holds the amount of damage this bullet deals.
    /// </summary>
    protected int damage = 0;

    private GameObject bulletDecal;

    private float decalTimeBeforeFadeOut;

    private float decalEffectLifetime;

    private GameObject hitEffect;

    private float hitEffectLifetime;*/

    public void InitializeBullet(Vector3 target, GunData gunData)
    {
        this.gunData = gunData;
        SetBullet(target, gunData.TimeToDestroy, gunData.BulletVelocity);
    }

    /*
    public void InitializeBullet(Vector3 target, int damage, float bulletVelocity, GameObject bulletDecal, float decalTimeBeforeFadeOut, float decalEffectLifetime, GameObject hitEffect, float hitEffectLifetime, float timeToDestroy)
    {
        this.damage = damage;
        this.bulletDecal = bulletDecal;
        this.decalTimeBeforeFadeOut = decalTimeBeforeFadeOut;
        this.decalEffectLifetime = decalEffectLifetime;
        this.hitEffect = hitEffect;
        this.hitEffectLifetime = hitEffectLifetime;

        SetBullet(target, timeToDestroy, bulletVelocity);
    }*/

    private void SetBullet(Vector3 target, float timeToDestroy, float bulletVelocity)
    {
        GetComponent<Rigidbody>().velocity = (target - transform.position).normalized * bulletVelocity;
        transform.LookAt(target);

        DestroyTimer(timeToDestroy);
    }

    private void DestroyTimer(float timeToDestroy)
    {
        Destroy(gameObject, timeToDestroy);
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
        ContactPoint contact = other.contacts[0];

        GameObject decal = Instantiate(gunData.BulletDecal, contact.point + contact.normal * 0.1f, Quaternion.LookRotation(contact.normal), other.gameObject.transform);
        decal.transform.localScale = gunData.BulletDecalSize * Vector3.one;
        decal.GetComponent<DecalBehaviour>().StartFadeOut(gunData.DecalEffectLifetime, gunData.DecalTimeBeforeFadeOut);
        GameObject particalEffect = Instantiate(gunData.HitEffect, contact.point + contact.normal * 0.2f, Quaternion.LookRotation(contact.normal));

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
            damagable.UpdateHealth(-gunData.Damage);
        }

        Destroy(gameObject);
    }
}
