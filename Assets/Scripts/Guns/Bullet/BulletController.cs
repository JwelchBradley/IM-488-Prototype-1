/*****************************************************************************
// File Name :         BulletController.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the movement and collisions of bullets.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    protected GunData gunData;

    public void InitializeBullet(Vector3 target, GunData gunData)
    {
        this.gunData = gunData;
        SetBullet(target, gunData.TimeToDestroy, gunData.BulletVelocity);
    }

    protected virtual void SetBullet(Vector3 target, float timeToDestroy, float bulletVelocity)
    {
        GetComponent<Rigidbody>().velocity = (target - transform.position).normalized * bulletVelocity;
        transform.LookAt(target);

        DestroyTimer(timeToDestroy);
    }

    private IEnumerator DestroyTimer(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Calls for the collision event to happen.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player Bullet")))
        {

        }
        else
        {
            SpawnParticleEffect(other, true);
            CollisionEvent(other);
        }
    }

    /// <summary>
    /// Handles what ever is supposed to happen during this bullets collision.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void CollisionEvent(Collision other)
    {
        ContactPoint contact = other.contacts[0];

        SpawnDecal(other, contact);

        Collider otherCol = other.gameObject.GetComponentInChildren<Collider>();

        if(otherCol != null)
        {
            PlayHitSound(contact.point, otherCol.sharedMaterial);
        }
        else
        {
            PhysicMaterial physicsMat = null;
            PlayHitSound(contact.point, physicsMat);
        }

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.UpdateHealth(-gunData.Damage);
        }

        gameObject.SetActive(false);
    }

    protected void SpawnDecal(Collision other, ContactPoint contact)
    {
        GameObject decal = gunData.hitDecalObjectPool.SpawnObj(contact.point + contact.normal * 0.01f, Quaternion.LookRotation(contact.normal));
        decal.transform.parent = null;
        decal.transform.localScale = gunData.BulletDecalSize * Vector3.one;
        decal.transform.parent = other.gameObject.transform;
        decal.GetComponent<DecalBehaviour>().StartFadeOut(gunData.DecalEffectLifetime, gunData.DecalTimeBeforeFadeOut);
    }

    protected void SpawnParticleEffect(Collision other, bool shouldSetParent)
    {
        ContactPoint contact = other.contacts[0];
        if (shouldSetParent)
        gunData.hitEffectObjectPool.SpawnObj(contact.point + contact.normal * .1f, Quaternion.LookRotation(contact.normal), other.gameObject.transform);
        else
            gunData.hitEffectObjectPool.SpawnObj(contact.point + contact.normal * .1f, Quaternion.LookRotation(contact.normal));
    }

    public void PlayHitSound(Vector3 pos, PhysicMaterial physicsMat)
    {
        //GameObject hitSoundPlayer = Instantiate(Resources.Load("HitSoundPlayer", typeof(GameObject)), pos, Quaternion.identity) as GameObject;

        AudioSource audio = gunData.hitSoundObjectPool.SpawnObj(pos, Quaternion.identity).GetComponent<AudioSource>();
        //Destroy(hitSoundPlayer, 3);

        //string matKey = other.gameObject.GetComponent<Renderer>().material.name;
        // Plays hitSound based off of the targets material
        //gunData.HitSound.AddHitSoundData();

        if (physicsMat != null && HitSoundData.Sound.TryGetValue(physicsMat, out AudioClip aud))
        {
            audio.clip = aud;
            audio.Play();
        }
        else
        {
            audio.clip = gunData.HitSound.DefaultSound;
            audio.Play();
        }
    }
}
