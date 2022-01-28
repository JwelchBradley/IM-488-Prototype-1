/*****************************************************************************
// File Name :         Ability.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Holds data for abilities
*****************************************************************************/
using UnityEditor;
using UnityEngine;
using MyBox;

[CreateAssetMenu(fileName = "New Ability", menuName = "Game Data/Ability")]
public class Ability : ScriptableObject
{
    [HideInInspector]
    public Camera mainCam;

    #region Visuals
    [Header("Visuals")]
    [Tooltip("The name of the ability (note: must be the same as the script name")]
    [SerializeField]
    private new string name;

    public string Name
    {
        get => name;
    }

    [Tooltip("The icon used for the UI of abilities")]
    [SerializeField]
    private Sprite icon;

    public Sprite Icon
    {
        get => icon;
    }
    #endregion

    #region Stats
    [Header("Stats")]
    #region Usage Time
    [Tooltip("How many charges this ability has")]
    [Range(1, 3)]
    [SerializeField]
    private int charges = 1;

    public int Charges
    {
        get => charges;
    }

    [Tooltip("How fast a single charge refreshes")]
    [Range(1.0f, 40.0f)]
    [SerializeField]
    private float cooldown = 4.0f;

    public float Cooldown
    {
        get => cooldown;
    }

    [Tooltip("How long the ability lasts for")]
    [Range(0.0f, 50.0f)]
    [SerializeField]
    private float duration = 2.0f;

    public float Duration
    {
        get => duration;
    }
    #endregion

    #region Cast values
    [Header("Cast Stats")]
    //[Space(20)]
    [Tooltip("Holds true if the player can shoot while casting")]
    [SerializeField] private bool canShootDuringCast = false;

    /// <summary>
    /// Holds true if the player can shoot while casting.
    /// </summary>
    public bool CanShootDuringCast
    {
        get => canShootDuringCast;
    }

    [Tooltip("How long it tasks the ability to be cast")]
    [Range(0.0f, 5.0f)]
    [SerializeField]
    private float castStartupTime = 0.0f;

    public float CastStartupTime
    {
        get => castStartupTime;
    }

    [ConditionalField("castStartupTime", true, 0)]
    [Tooltip("Holds true if the user can move while the cast is starting up")]
    [SerializeField]
    private bool movementDuringCastStartup = false;

    public bool MovementDuringCastStartup
    {
        get => movementDuringCastStartup;
    }

    [Space(10)]
    [Tooltip("How long will the user be in the casting")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float castDuration = 0.0f;

    public float CastDuration
    {
        get => castDuration;
    }

    [ConditionalField("castDuration", true, 0.0f)]
    [Tooltip("Holds true if the user can move while casting")]
    [SerializeField]
    private bool moveDuringCast = false;

    public bool MoveDuringCast
    {
        get => moveDuringCast;
    }

    [Space(10)]
    [Tooltip("How long after casting before the user can take other actions")]
    [Range(0.0f, 5.0f)]
    [SerializeField]
    private float uncastTime = 0.0f;

    public float UncastTime
    {
        get => uncastTime;
    }

    [ConditionalField("uncastTime", true, 0.0f)]
    [Tooltip("Holds true if the user can move while uncasting")]
    [SerializeField]
    private bool moveDuringUncast = false;

    public bool MovementDuringUncast { get => moveDuringUncast; }

    [ConditionalField("moveDuringUncast")]
    [Tooltip("How fast the player can move while casting")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float newUncastMoveSpeed = 5.0f;

    public float NewUncastMoveSpeed { get => newUncastMoveSpeed; }

    [Tooltip("Holds true if the player can zoom it while using this ability")]
    [SerializeField]
    private bool canZoomWhileCasting = false;

    /// <summary>
    /// Holds true if the player can zoom it while using this ability.
    /// </summary>
    public bool CanZoomWhileCasting
    {
        get => canZoomWhileCasting;
    }
    #endregion

    #region Specific
    [Header("Specific Values")]
    [Tooltip("The type of ability this is")]
    public type abilityType = type.gravity;

    public enum type
    {
        gravity,
        freeze,
        shield
    }

    #region Gravity Specific
    [ConditionalField("abilityType", type.gravity)]
    [Tooltip("Layers players can push or pull towards them")]
    [SerializeField]
    private LayerMask pushPullableMask;

    /// <summary>
    /// Layers players can push or pull towards them.
    /// </summary>
    public LayerMask PushPullableMask
    {
        get => pushPullableMask;
    }

    [ConditionalField("abilityType", type.gravity)]
    [Range(0.0f, 100.0f)]
    [Tooltip("How fast the player pulls something towards them")]
    [SerializeField]
    private float pushPullSpeed = 5.0f;

    /// <summary>
    /// How fast the player pulls something towards them.
    /// </summary>
    public float PushPullSpeed
    {
        get => pushPullSpeed;
    }

    [ConditionalField("abilityType", type.gravity)]
    [Range(0.0f, 20.0f)]
    [Tooltip("How far off the player cursor will there be an aim assist to")]
    [SerializeField]
    private float aimAssist;

    public float AimAssist
    {
        get => aimAssist;
    }

    [ConditionalField("abilityType", type.gravity)]
    [Range(5.0f, 100.0f)]
    [Tooltip("How far away can objects be pulled or pushed from")]
    [SerializeField]
    private float pushPullDist;

    public float PushPullDist
    {
        get => pushPullDist;
    }

    [ConditionalField("abilityType", type.gravity)]
    [Tooltip("How much the distance from the current tether point affect follow speed")]
    [Range(0.0f, 1000.0f)]
    [SerializeField]
    private float distMod = 100.0f;

    /// <summary>
    /// How much the distance from the current tether point affect follow speed.
    /// </summary>
    public float DistMod
    {
        get => distMod;
    }

    [ConditionalField("abilityType", type.gravity)]
    [Tooltip("How fast the object follows the tether")]
    [Range(0.0f, 10000.0f)]
    [SerializeField]
    private float followTetherSpeed = 1000.0f;

    public float FollowTetherSpeed
    {
        get => followTetherSpeed;
    }

    [ConditionalField("abilityType", type.gravity)]
    [Tooltip("The material of the line")]
    [SerializeField]
    private Material lineMaterial;

    /// <summary>
    /// The material of the line.
    /// </summary>
    public Material LineMaterial
    {
        get => lineMaterial;
    }

    [ConditionalField("abilityType", type.gravity)]
    [Tooltip("The width of the linderenderer and its start point")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float lineStartWidth = 0.1f;

    /// <summary>
    /// The width curve of the linerenderer.
    /// </summary>
    public float LineStartWidth
    {
        get => lineStartWidth;
    }

    [ConditionalField("abilityType", type.gravity)]
    [Tooltip("The width of the linderenderer and its end point")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float lineEndWidth = 0.2f;

    /// <summary>
    /// The width curve of the linerenderer.
    /// </summary>
    public float LindEndWidth
    {
        get => lineEndWidth;
    }

    [SerializeField] private AnimationCurve lineWidthCurve;

    public AnimationCurve LineWidthCurve
    {
        get => lineWidthCurve;
    }
    #endregion

    #region Shield Specific
    [ConditionalField("abilityType", type.shield)]
    [Range(100, 10000)]
    [Tooltip("How much health the shield has")]
    public int health = 1000;

    [ConditionalField("abilityType", type.shield)]
    [Range(0.0f, 20.0f)]
    [Tooltip("How large the shield is")]
    public float size = 5.0f;

    [ConditionalField("abilityType", type.shield)]
    [Tooltip("The shield that will be spawned")]
    public GameObject shield;
    #endregion
    #endregion
    #endregion
}