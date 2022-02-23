using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShootingEnemy : BaseEnemy
{
    [SerializeField] private GameObject lookObject;

    [SerializeField] private Transform bulletSpawnPos;

    private AudioSource aud;

    private float timeLastShot = 0;

    private Coroutine shootRoutine;

    private bool notFrozen = true;


    private ShootingEnemy self;

    private bool shouldShoot = false;

    protected override void Awake()
    {
        base.Awake();
        aud = GetComponent<AudioSource>();
        self = GetComponent<ShootingEnemy>();
        enemyData.GunData.SpawnObjectPool();
    }

    protected virtual void LookAtPlayer()
    {
        lookObject.transform.LookAt(playerReference.transform.position);
    }

    private void FixedUpdate()
    {
        if(notFrozen && (bulletSpawnPos.position - playerReference.transform.position).sqrMagnitude < enemyData.Range)
        {
            LookAtPlayer();
            shouldShoot = true;
            if (shootRoutine == null)
            {
                shootRoutine = StartCoroutine(ShootRoutine());
            }
        }
        else if(shootRoutine != null)
        {
            shouldShoot = false;
            //StopAllCoroutines();
            //shootRoutine = null;
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (self.enabled && shouldShoot)
        {
            ShootBullet();
            yield return new WaitForSeconds(1 / enemyData.GunData.FireRate);
        }

        shouldShoot = false;
        shootRoutine = null;
    }

    /// <summary>
    /// Shoots the bullets at the reticle.
    /// </summary>
    protected void ShootBullet()
    {
        //GameObject bullet = Instantiate(enemyData.GunData.Bullet, bulletSpawnPos.position, Quaternion.identity);
        //BulletController bulletController = bullet.GetComponent<BulletController>();

        BulletController bulletController = null;
        enemyData.GunData.objectPool.SpawnObj(bulletSpawnPos.position, Quaternion.identity, ref bulletController);
        aud.PlayOneShot(enemyData.GunData.ShootShound);

        Vector3 target = playerReference.transform.position;

        bulletController.InitializeBullet(target, enemyData.GunData);
    }
}
