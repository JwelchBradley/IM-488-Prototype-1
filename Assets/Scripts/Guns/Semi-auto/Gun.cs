/*****************************************************************************
// File Name :         Gun.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the shooting of guns.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    #region Variables
    [Tooltip("The data for this gun")]
    [SerializeField]
    protected GunData gunData;

    /// <summary>
    /// The time that the last shot was taken.
    /// </summary>
    protected float timeLastShot;

    /// <summary>
    /// Holds true if a bullet has been queued.
    /// </summary>
    protected bool shotQueued = false;

    /// <summary>
    /// Picks a target 1000 units away from the player if there is not target on the reticle.
    /// </summary>
    private float bulletDist = 1000.0f;

    protected float overheatAmount = 0.0f;

    private bool overheated = false;

    private Coroutine reduceOverheat;

    private Image overheatBar;

    private LayerMask shootMask;

    #region References
    /// <summary>
    /// The transform of the scenes main camera.
    /// </summary>
    private Transform mainCam;

    /// <summary>
    /// The audiosource of this gameobject.
    /// </summary>
    private AudioSource aud;

    /// <summary>
    /// The position that bullets will be spawned from.
    /// </summary>
    private Transform bulletSpawnPos;

    private Rigidbody playerRb;
    #endregion
    #endregion

    /// <summary>
    /// Gets the main cameras transform.
    /// </summary>
    private void Awake()
    {
        mainCam = Camera.main.transform;
        aud = GetComponent<AudioSource>();
        bulletSpawnPos = transform.Find("Bullet Spawn Pos");
        InitializeShootMask();
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        overheatBar = GameObject.Find("Overheat").GetComponent<Image>();
    }
    
    private void InitializeShootMask()
    {
        shootMask = ~0;
        shootMask = shootMask ^ (1 << LayerMask.NameToLayer("Player"));
        shootMask = shootMask ^ (1 << LayerMask.NameToLayer("Player Bullet"));
        shootMask = shootMask ^ (1 << LayerMask.NameToLayer("Shield"));
    }

    /// <summary>
    /// Shoots bullet.
    /// </summary>
    public virtual void Shoot(bool shouldShoot)
    {
        if (shouldShoot)
        {
            float potentialShootTime = timeLastShot + (1 / gunData.FireRate);

            if (potentialShootTime < Time.time)
            {
                ShootBullet();
            }
            else if(!shotQueued && gunData.TapStaggerTime + Time.time >= potentialShootTime)
            {
                shotQueued = true;
                Invoke("ShootBullet", potentialShootTime - Time.time);
            }
        }
    }

    public void Shoot(GameObject overrideBullet, Vector3 target, AudioClip fireSound, float stunDuration, GunData empGunData)
    {
        GameObject overrideBulletRef = Instantiate(overrideBullet, bulletSpawnPos.position, Quaternion.identity);
        EMPBulletController bulletController = overrideBulletRef.GetComponent<EMPBulletController>();
        aud.PlayOneShot(fireSound);

        bulletController.InitializeBullet(target, empGunData);
        bulletController.StunDuration = stunDuration;
    }

    /// <summary>
    /// Shoots the bullets at the reticle.
    /// </summary>
    protected void ShootBullet()
    {
        if (overheated)
            return;

        // Spawns the bullet
        RaycastHit hit;
        GameObject bullet = Instantiate(gunData.Bullet, bulletSpawnPos.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        timeLastShot = Time.time;
        aud.PlayOneShot(gunData.ShootShound);
        shotQueued = false;

        if(gunData.OverheatPerShot != 0)
        IncreaseOverheatAmount();

        playerRb.AddForce(-mainCam.forward.normalized * gunData.PushBackForce);

        Vector3 target;

        // Checks if there is a target on the reticle.
        if (Physics.Raycast(mainCam.position, mainCam.forward, out hit, Mathf.Infinity, shootMask))
        {
            target = hit.point;
        }
        else
        {
            target = mainCam.position + mainCam.forward * bulletDist;
        }

        bulletController.InitializeBullet(target, gunData);
    }

    private void IncreaseOverheatAmount()
    {
        if(reduceOverheat != null)
        StopCoroutine(reduceOverheat);

        overheatAmount += gunData.OverheatPerShot;

        if(overheatAmount >= gunData.OverheatLimit)
        {
            overheated = true;
        }

        reduceOverheat = StartCoroutine(ReduceOverheat());
        ChangeOverheatBar();
    }

    private IEnumerator ReduceOverheat()
    {
        yield return new WaitForSeconds(gunData.WaitBeforeOverheatReduction);

        while(overheatAmount > 0)
        {
            overheatAmount -= gunData.OverheatReductionAmount;
            ChangeOverheatBar();
            yield return new WaitForSeconds(gunData.OverheatReductionTickRate);
        }

        overheatAmount = 0;
        overheated = false;
    }

    private void ChangeOverheatBar()
    {
        overheatBar.fillAmount = (overheatAmount / gunData.OverheatLimit) / 2;
        overheatBar.color = Color.Lerp(Color.white, Color.red, overheatBar.fillAmount * 2);
    }
}
