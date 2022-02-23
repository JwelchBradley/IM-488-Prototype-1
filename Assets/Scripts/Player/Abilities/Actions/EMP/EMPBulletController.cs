using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPBulletController : BulletController
{
    private float stunDuration;

    private Renderer renderer;

    private void OnEnable()
    {
        renderer = GetComponent<Renderer>();
    }

    public float StunDuration
    {
        set
        {
            stunDuration = value;
        }
    }

    protected override void CollisionEvent(Collision other)
    {
        gunData.SpawnObjectPool();

        if (other.gameObject.CompareTag("Bullet"))
        {
            return;
        }

        ContactPoint contact = other.contacts[0];

        SpawnDecal(other, contact);
        gunData.hitEffectObjectPool.SpawnObj(contact.point + contact.normal * .1f, Quaternion.LookRotation(contact.normal), other.gameObject.transform);

        Collider otherCol = other.gameObject.GetComponentInChildren<Collider>();

        if (otherCol != null)
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

        if (other.gameObject.TryGetComponent(out BaseEnemy be))
        {
            be.enabled = false;
            StartCoroutine(reenableEnemy(be));
        }

        renderer.enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    } 

    private IEnumerator reenableEnemy(BaseEnemy be)
    {
        yield return new WaitForSeconds(stunDuration);
        if(be != null)
        be.enabled = true;
        Destroy(gameObject);
    }
}
