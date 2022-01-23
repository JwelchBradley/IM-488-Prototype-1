using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    [Tooltip("How fast this gun shoots")]
    [Range(0.0f, 50.0f)]
    [SerializeField]
    private float fireRate = 5.0f;

    [Tooltip("How much damage this gun deals")]
    [Range(0, 100)]
    [SerializeField]
    private float damage = 5;

    [Tooltip("The bullet that this gun shoots")]
    [SerializeField]
    private GameObject bullet;

    /// <summary>
    /// Shoots bullet.
    /// </summary>
    public virtual void Shoot()
    {
        // Shoots bullet
    }

    protected void ShootBullet(Vector3 targetPos)
    {

    }
}
