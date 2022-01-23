/*****************************************************************************
// File Name :         Gun.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the shooting of guns.
*****************************************************************************/
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    #region Gun Stats
    [Header("Gun Stats")]
    [Tooltip("How fast this gun shoots")]
    [Range(0.0f, 50.0f)]
    [SerializeField]
    protected float fireRate = 5.0f;

    /// <summary>
    /// The time that the last shot was taken.
    /// </summary>
    protected float timeLastShot;

    [Tooltip("How much damage this gun deals")]
    [Range(0, 100)]
    [SerializeField]
    protected int damage = 5;

    [Tooltip("How inaccurate this gun is [Not Yet Implemented]")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    protected float bulletInaccuracy;

    [Tooltip("How fast this guns projectiles move")]
    [Range(0.0f, 100.0f)]
    [SerializeField]
    protected float bulletVelocity = 10.0f;

    [Tooltip("How long after tapping will the gun hold a reference to shoot")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    protected float tapStaggerTime = 0.2f;

    protected bool shotQueued = false;
    #endregion

    [Header("Visuals")]
    [Tooltip("The bullet that this gun shoots")]
    [SerializeField]
    private GameObject bullet;

    [Tooltip("The position that bullets will be spawned from")]
    [SerializeField]
    private GameObject bulletSpawnPos;

    [Header("Audio")]
    [Tooltip("The sound made when the gun is fired")]
    [SerializeField]
    private AudioClip shootShound;

    /// <summary>
    /// Picks a target 1000 units away from the player if there is not target on the reticle.
    /// </summary>
    private float bulletDist = 1000.0f;

    /// <summary>
    /// The transform of the scenes main camera.
    /// </summary>
    private Transform mainCam;

    /// <summary>
    /// The audiosource of this gameobject.
    /// </summary>
    private AudioSource aud;

    /// <summary>
    /// Gets the main cameras transform.
    /// </summary>
    private void Awake()
    {
        mainCam = Camera.main.transform;
        aud = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Shoots bullet.
    /// </summary>
    public virtual void Shoot(bool shouldShoot)
    {
        if (shouldShoot)
        {
            float potentialShootTime = timeLastShot + (1 / fireRate);

            if (potentialShootTime < Time.time)
            {
                ShootBullet();
            }
            else if(!shotQueued && tapStaggerTime + Time.time >= potentialShootTime)
            {
                shotQueued = true;
                Invoke("ShootBullet", potentialShootTime - Time.time);
            }
        }
    }

    /// <summary>
    /// Shoots the bullets at the reticle.
    /// </summary>
    protected void ShootBullet()
    {
        // Spawns the bullet
        RaycastHit hit;
        GameObject bullet = Instantiate(this.bullet, bulletSpawnPos.transform.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        timeLastShot = Time.time;
        aud.PlayOneShot(shootShound);
        shotQueued = false;

        // Checks if there is a target on the reticle.
        if (Physics.Raycast(mainCam.position, mainCam.forward, out hit, Mathf.Infinity))
        {    
            bulletController.Target = hit.point;
        }
        else
        {
            bulletController.Target = mainCam.position + mainCam.forward * bulletDist;
        }

        // Sets values of the bullet
        bulletController.Damage = damage;
        bulletController.Speed = bulletVelocity;
        bullet.transform.LookAt(bulletController.Target);
    }
}
