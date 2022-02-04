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

    bool hasCollided = false;

    public void InitializeBullet(Vector3 target, GunData gunData)
    {
        this.gunData = gunData;
        SetBullet(target, gunData.TimeToDestroy, gunData.BulletVelocity);
    }

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
        if (!hasCollided)
        {
            CollisionEvent(other);
            hasCollided = true;
        }
    }

    /// <summary>
    /// Handles what ever is supposed to happen during this bullets collision.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void CollisionEvent(Collision other)
    {
        ContactPoint contact = other.contacts[0];

        GameObject decal = Instantiate(gunData.BulletDecal, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));
        decal.transform.localScale = gunData.BulletDecalSize * Vector3.one;
        decal.transform.parent = other.gameObject.transform;
        decal.GetComponent<DecalBehaviour>().StartFadeOut(gunData.DecalEffectLifetime, gunData.DecalTimeBeforeFadeOut);
        GameObject particalEffect = Instantiate(gunData.HitEffect, contact.point + contact.normal * .1f, Quaternion.LookRotation(contact.normal), other.gameObject.transform);

        GameObject hitSoundPlayer = Instantiate(Resources.Load("HitSoundPlayer", typeof(GameObject)), contact.point, Quaternion.identity) as GameObject;
        AudioSource audio = hitSoundPlayer.GetComponent<AudioSource>();

        Destroy(hitSoundPlayer, 3);
        Destroy(decal, gunData.DecalEffectLifetime);
        Destroy(particalEffect, gunData.HitEffectLifetime);

        //string matKey = other.gameObject.GetComponent<Renderer>().material.name;
        // Plays hitSound based off of the targets material
        gunData.HitSound.AddHitSoundData();

        PhysicMaterial physicsMat = other.gameObject.GetComponentInChildren<Collider>().sharedMaterial;
        if(physicsMat != null && HitSoundData.Sound.TryGetValue(physicsMat, out AudioClip aud)){
            audio.clip = aud;
            audio.Play();
        }
        else
        {
            audio.clip = gunData.HitSound.DefaultSound;
            audio.Play();
        }

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.UpdateHealth(-gunData.Damage);
        }

        Destroy(gameObject);
    }
}
