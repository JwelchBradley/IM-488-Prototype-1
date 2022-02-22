using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Game Data/Gun")]
public class GunData : ScriptableObject
{
    [HideInInspector]
    public ObjectPool objectPool;
    [HideInInspector]
    public ObjectPool hitEffectObjectPool;
    [HideInInspector]
    public ObjectPool hitDecalObjectPool;
    [HideInInspector]
    public ObjectPool hitSoundObjectPool;

    public void SpawnObjectPool()
    {
        if (objectPool == null)
        {
            objectPool = Instantiate(ObjectPoolToSpawn, GameObject.Find("ObjectPools").transform).GetComponent<ObjectPool>();
            objectPool.isBulletController = true;
            objectPool.PoolObjects(Bullet, ObjectPoolAmount);
            hitEffectObjectPool = Instantiate(ObjectPoolToSpawn, GameObject.Find("ObjectPools").transform).GetComponent<ObjectPool>();
            hitEffectObjectPool.PoolObjects(hitEffect, objectPoolAmount);
            hitDecalObjectPool = Instantiate(ObjectPoolToSpawn, GameObject.Find("ObjectPools").transform).GetComponent<ObjectPool>();
            hitDecalObjectPool.PoolObjects(BulletDecal, objectPoolAmount);
            hitSoundObjectPool = Instantiate(ObjectPoolToSpawn, GameObject.Find("ObjectPools").transform).GetComponent<ObjectPool>();
            hitSoundObjectPool.PoolObjects(Instantiate(Resources.Load("HitSoundPlayer", typeof(GameObject))) as GameObject, objectPoolAmount);
        }
    }

    #region Gun Stats
    [Header("Gun Stats")]
    [Tooltip("How fast this gun shoots")]
    [Range(0.0f, 50.0f)]
    [SerializeField]
    private float fireRate = 5.0f;

    /// <summary>
    /// The firerate of this gun.
    /// </summary>
    public float FireRate
    {
        get => fireRate;
    }

    [Tooltip("How much damage this gun deals")]
    [Range(0, 100)]
    [SerializeField]
    private int damage = 5;

    /// <summary>
    /// The damage of this gun.
    /// </summary>
    public int Damage
    {
        get => damage;
    }

    [Tooltip("How long after tapping will the gun hold a reference to shoot")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float tapStaggerTime = 0.2f;

    /// <summary>
    /// The tapStaggerTime of this gun.
    /// </summary>
    public float TapStaggerTime
    {
        get => tapStaggerTime;
    }

    #region Overheat
    [SerializeField]
    private float overheatLimit = 100.0f;

    public float OverheatLimit
    {
        get => overheatLimit;
    }

    [SerializeField]
    private float overheatPerShot = 0.0f;

    public float OverheatPerShot
    {
        get => overheatPerShot;
    }

    [SerializeField]
    private float waitBeforeOverheatReduction = 2.0f;

    public float WaitBeforeOverheatReduction
    {
        get => waitBeforeOverheatReduction;
    }

    [SerializeField]
    private float overheatReductionAmount = 1.0f;

    public float OverheatReductionAmount
    {
        get => overheatReductionAmount;
    }

    [SerializeField]
    private float overheatReducitonTickRate = 0.02f;

    public float OverheatReductionTickRate
    {
        get => overheatReducitonTickRate;
    }
    #endregion
    #endregion

    #region ADS
    [Header("ADS")]
    #region Camera Settings
    [Tooltip("How zoomed in the ADS is")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float zoomMod = 2.0f;
    #endregion
    #endregion

    #region Sound
    [Header("Audio")]
    [Tooltip("The sound made when the gun is fired")]
    [SerializeField]
    private AudioClip shootShound;

    /// <summary>
    /// The shootShound of this gun.
    /// </summary>
    public AudioClip ShootShound
    {
        get => shootShound;
    }

    [Tooltip("Holds the hit sounds for this guns bullets")]
    [SerializeField]
    private HitSoundData hitSound;

    /// <summary>
    /// The hitSound of this gun.
    /// </summary>
    public HitSoundData HitSound
    {
        get => hitSound;
    }
    #endregion

    #region Bullet
    [Header("Bullet Stats")]
    [Header("--------Bullet--------")]
    [Tooltip("How inaccurate this gun is [Not Yet Implemented]")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float bulletInaccuracy;

    /// <summary>
    /// The bulletInaccuracy of this gun.
    /// </summary>
    public float BulletInaccuracy
    {
        get => bulletInaccuracy;
    }

    [Tooltip("How fast this guns projectiles move")]
    [Range(0.0f, 500.0f)]
    [SerializeField]
    private float bulletVelocity = 10.0f;

    /// <summary>
    /// The bulletVelocity of this gun.
    /// </summary>
    public float BulletVelocity
    {
        get => bulletVelocity;
    }

    [Tooltip("How much force is applied to pushback the character shooting this gun")]
    [SerializeField] private float pushBackForce = 0.0f;

    public float PushBackForce
    {
        get => pushBackForce;
    }

    [Header("Bullet Visuals")]
    [Tooltip("The bullet that this gun shoots")]
    [SerializeField]
    private GameObject bullet;

    /// <summary>
    /// The bullet of this gun.
    /// </summary>
    public GameObject Bullet
    {
        get => bullet;
    }

    [SerializeField]
    private GameObject objectPoolToSpawn;

    public GameObject ObjectPoolToSpawn
    {
        get => objectPoolToSpawn;
    }

    [SerializeField]
    private int objectPoolAmount = 40;

    public int ObjectPoolAmount
    {
        get => objectPoolAmount;
    }

    [Tooltip("The decal spawned against the location this hits")]
    [SerializeField]
    private GameObject bulletDecal;

    public GameObject BulletDecal
    {
        get => bulletDecal;
    }

    [Tooltip("How large the decal is")]
    [SerializeField] private float bulletDecalSize = 0.05f;

    /// <summary>
    /// How large the decal is.
    /// </summary>
    public float BulletDecalSize
    {
        get => bulletDecalSize;
    }

    [Space(20)]
    [Tooltip("How long before the decal is despawned")]
    [Range(0.0f, 20.0f)]
    [SerializeField]
    private float decalEffectLifetime = 5.0f;

    public float DecalEffectLifetime
    {
        get => decalEffectLifetime;
    }

    [Tooltip("How long before the decal starts to fade out")]
    [Range(0.0f, 20.0f)]
    [SerializeField]
    private float decalTimeBeforeFadeOut = 2.0f;

    public float DecalTimeBeforeFadeOut
    {
        get => decalTimeBeforeFadeOut;
    }

    [Space(20)]
    [Tooltip("The partical effect spawned at the collision point")]
    [SerializeField]
    private GameObject hitEffect;

    public GameObject HitEffect
    {
        get => hitEffect;
    }

    [Tooltip("How long before the hit effect is despawned")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float hitEffectLifetime = 1.0f;

    public float HitEffectLifetime
    {
        get => hitEffectLifetime;
    }

    [Tooltip("How much time the bullet has before self destructing")]
    [SerializeField]
    private float timeToDestroy = 5.0f;

    public float TimeToDestroy
    {
        get => timeToDestroy;
    }
    #endregion
}


