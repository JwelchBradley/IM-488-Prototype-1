using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    [Tooltip("How fast this gun shoots")]
    [Range(0.0f, 50.0f)]
    [SerializeField]
    protected float fireRate = 5.0f;

    [Tooltip("How much damage this gun deals")]
    [Range(0, 100)]
    [SerializeField]
    protected int damage = 5;

    [Tooltip("How inaccurate this gun is")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    protected float bulletInaccuracy;

    [Header("Visuals")]
    [Tooltip("The bullet that this gun shoots")]
    [SerializeField]
    private GameObject bullet;

    [Tooltip("The position that bullets will be spawned from")]
    [SerializeField]
    private GameObject bulletSpawnPos;
    
    [Tooltip("How fast this guns projectiles move")]
    [Range(0.0f, 100.0f)]
    [SerializeField]
    protected float bulletVelocity = 10.0f;

    private float bulletDist = 25.0f;

    private Transform mainCam;

    private void Awake()
    {
        mainCam = Camera.main.transform;
    }

    /// <summary>
    /// Shoots bullet.
    /// </summary>
    public virtual void Shoot(bool shouldShoot)
    {
        // Shoots bullet

        if(shouldShoot)
        ShootBullet();
    }

    protected void ShootBullet()
    {
        RaycastHit hit;
        GameObject bullet = Instantiate(this.bullet, bulletSpawnPos.transform.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Speed = bulletVelocity;

        if (Physics.Raycast(mainCam.position, mainCam.forward, out hit, Mathf.Infinity))
        {    
            bulletController.Target = hit.point;
        }
        else
        {
            bulletController.Target = mainCam.position + mainCam.forward * bulletDist;
        }

        bulletController.Damage = damage;
        bullet.transform.LookAt(bulletController.Target);
    }
}
