/*****************************************************************************
// File Name :         ThirdPersonController.cs
// Author :            Jacob Welch, Jessica Barthelt
// Creation Date :     21 January 2022
//
// Brief Description : Handles the movement of the player.
*****************************************************************************/
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(KeybindInputHandler))]
public class ThirdPersonController : MonoBehaviour, IDamagable
{
	#region Variables
	#region Player
	/// <summary>
	/// Overall player speed (x and y axis).
	/// </summary>
	private float currentSpeed;

	private float animationBlend;

	private enum moveState
	{
		normal,
		fast,
		dash,
		casting,
		nomovecasting,
		ADS
	}

	private moveState currentMoveState = moveState.normal;

	#region Flat Movement
	[Header("Horizontal Movement")]
	[Header("-------------Normal Movement-------------")]
	[Tooltip("Move speed of the character in m/s")]
	[Range(0, 50)]
	[SerializeField]
	private float normalSpeedCap = 2.0f;

	private float normalSpeedCapSquared;

	[Tooltip("Acceleration and deceleration of the character ground movement")]
	[Range(100.0f, 5000.0f)]
	[SerializeField]
	private float speedChangeRate = 400;

	#region Character Rotation
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(1.0f, 10.0f)]
	[SerializeField]
	private float rotationSmoothTime = 2.0f;
	#endregion
	#endregion

	#region Vertical Movement

	[Header("Vertical Movement")]
	[Tooltip("How fast the player changes their vertical speed")]
	[Range(100.0f, 5000.0f)]
	[SerializeField]
	private float verticalSpeedChangeRate = 400;
	#endregion

	#region Dash
	[Header("Dash")]
	[Tooltip("How fast the player is while dashing")]
	[Range(0.0f, 200.0f)]
	[SerializeField] private float dashSpeedCap = 10.0f;

	/// <summary>
	/// The squared value of dashSpeedCap.
	/// </summary>
	private float dashSpeedCapSqr;

	[Tooltip("How fast the player accelerates to max dash speed")]
	[Range(5000.0f, 40000.0f)]
	[SerializeField] private float dashSpeedAcceleration = 10000;

	[Tooltip("How long the dash lasts")]
	[Range(0.0f, 1.0f)]
	[SerializeField]
	private float dashTime = 0.5f;

	[Tooltip("How long before the player can dash again")]
	[Range(0.0f, 2.0f)]
	[SerializeField] private float dashStaggerTime = 0.5f;

	[Tooltip("The velocity that the player is clamped to out of dashes")]
	[Range(0.0f, 30.0f)]
	[SerializeField] private float exitDashSpeedClamp;

	/// <summary>
	/// The velocity that the player is clamped to out of dashes.
	/// </summary>
	private float exitDashSpeedClampSquared;

	/// <summary>
	/// Holds true if the player can dash.
	/// </summary>
	private bool canDash = true;

	/// <summary>
	/// The time that the dash starts.
	/// </summary>
	private float startTime;
	#endregion

	#region FastMovement
	[Header("Fast move")]
	[Header("-------------Fast Movement-------------")]
	[Tooltip("Sprint speed of the character in m/s")]
	[Range(0, 100)]
	[SerializeField]
	private float fastSpeedCap = 5.335f;

	/// <summary>
	/// The squared value of the fast speed cap.
	/// </summary>
	private float fastSpeedCapSquared;

	[Tooltip("How fast the player gets to their forward speed")]
	[SerializeField] private float forwardAcceleration = 2.5f;

	[Tooltip("How fast the player gets to their strafe speed")]
	[SerializeField] private float strafeAcceleration = 2.0f;
	
	[Tooltip("How fast the player gets to their vertical fast speed")]
	[SerializeField] private float verticalAcceleration = 2.0f;

	[Tooltip("How fast the player rotates while fast moving")]
	[SerializeField] float rotationSpeed = 10;

	[Header("Speed Lines")]
	[Tooltip("How much the speed lines rotate while strafing in fast movement")]
	[SerializeField] private float speedLinesSidewaysRotation = 30;

	[Tooltip("How much rotation is vertically given.")]
	[SerializeField] private float speedLinesVerticalRotation = 15;

	[Tooltip("How much the lines offset on the x-axis")]
	[Range(0.0f, 5.0f)]
	[SerializeField] private float xPositionalOffset = 2.5f;

	[Tooltip("How much the lines offset on the y-axis")]
	[Range(0.0f, 5.0f)]
	[SerializeField] private float yPositionalOffset = 2.5f;

	/*
	[Tooltip("How much the camera looks ahead while strafing")]
	[SerializeField] private float strafeLookAhead = 1.0f;

	[Tooltip("How much the camera looks ahead while hovering")]
	[SerializeField] private float hoverLookAhead = 1.0f;
	*/
	/// <summary>
	/// Handles the switching on and off of speed line particles.
	/// </summary>
	public class MoveFastEvent : UnityEvent<bool> { }

	/// <summary>
	/// An instance of the MoveFastEvent.
	/// </summary>
	public MoveFastEvent MoveFast = new MoveFastEvent();
	#endregion

	#region Slow Movement
	[Header("Slow down movement rate")]
	[SerializeField] private float slowDownRate = 0.5f;
    #endregion
    #endregion

    #region Cinemachine
    [Header("Cinemachine")]
	[Header("-------------Aim-------------")]
	#region Aim
	#region Normal and ADS
	[Tooltip("The camera used for normal movement")]
	[SerializeField] private CinemachineVirtualCamera normalCam;

	[Tooltip("The camera used for ads movement")]
	[SerializeField] private CinemachineVirtualCamera adsCam;

	[Tooltip("The normal and ads target")]
	[SerializeField] private Transform cinemachineNormalADSTarget;

	[Tooltip("The lowest angle the player can look at")]
	[Range(-90, 50)]
	[SerializeField] private float bottomClamp = -90;

	[Tooltip("The highest angle the player can look at")]
	[Range(50, 90)]
	[SerializeField] private float topClamp = 90;
    #endregion

    #region Fast
    [Space(20)]
	[Tooltip("The target for fast movement")]
	[SerializeField] private Transform cinemachineCameraTarget;

	[SerializeField] private CinemachineVirtualCamera fastCam;

	[SerializeField] private GameObject speedLines;
    #endregion

	/// <summary>
	/// The brain for the camera.
	/// </summary>
	private CinemachineBrain camBrain;

	#region Sensitivity
	#region Fast
	private float xFastSens = 2;

	public float XFastSens
    {
        set
        {
			xFastSens = value/5;
        }
    }

	private float yFastSens = 2;

	public float YFastSens
    {
		set
		{
			yFastSens = value/5;
		}
	}
    #endregion

    #region Normal and ADS
    /// <summary>
    /// How sensitive the camera is in the x-axis.
    /// </summary>
    private float xSens = 1;

	/// <summary>
	/// The sensitivity the camera uses for the x-axis.
	/// </summary>
	public float XSens
    {
        set
        {
			xSens = value;
        }
    }

	/// <summary>
	/// The current target rotation of the cinemachine follow object.
	/// </summary>
	private float cinemachineTargetXRot;

	/// <summary>
	/// How sensitive the camera is in the y-axis.
	/// </summary>
	private float ySens = 1;

	/// <summary>
	/// The sensitivity the camera uses for the y-axis.
	/// </summary>
	public float YSens
	{
        set
        {
			ySens = value;
        }
	}

	/// <summary>
	/// The sensitivity mod for ads.
	/// </summary>
	private float adsAimMod = 1;

	/// <summary>
	/// The sensitivity mod for ads.
	/// </summary>
	public float ADSAimMod
    {
        set
        {
			adsAimMod = value;
        }
    }

	/// <summary>
	/// How sensitive the camera is in the y-axis.
	/// </summary>
	private float cinemachineTargetYRot;

    /// <summary>
    /// The threshold of mouse delta movement for a look calculation.
    /// </summary>
    private float lookAmountThreshold = 0.01f;
    #endregion
    #endregion
    #endregion
    #endregion

    #region Animation IDs
    [Header("Animation")]
	[Header("-------------Extras-------------")]
	[Tooltip("The pivot for the visuals")]
	[SerializeField] private Transform visuals;

	[Tooltip("The animator name for the speed animation")]
	[SerializeField]
	private string animSpeedName;

	/// <summary>
	/// The Animation ID for the player's blend speed.
	/// </summary>
	private int animIDSpeed;

	[Tooltip("The animator name for the motion speed animation")]
	[SerializeField]
	private string animMotionSpeedName;

	/// <summary>
	/// The Animation ID for the player's current speed.
	/// </summary>
	private int animIDMotionSpeed;
	#endregion

	#region Health
	[Header("Health")]
	[Tooltip("The amount of health the player starts with")]
	[SerializeField]
	[Range(0, 500)]
	private int startingHealth;

	/// <summary>
	/// The amount of health the player currently has.
	/// </summary>
	public int health;
	#endregion

	#region Gun
	/// <summary>
	/// The players gun.
	/// </summary>
	private Gun gun;

	/// <summary>
	/// Holds true if the player can shoot.
	/// </summary>
	private bool canShoot = true;
	#endregion

	#region Abilities
	/// <summary>
	/// The array of abilities the player has.
	/// </summary>
	private AbilityAction[] abilities;

	/// <summary>
	/// The array of abilities the player has.
	/// </summary>
	public AbilityAction[] Abilities
	{
		get => abilities;
	}
	#endregion

	#region Components
	/// <summary>
	/// The rigidbody of the player.
	/// </summary>
	private Rigidbody rb;

	/// <summary>
	/// The input manager for the player.
	/// </summary>
	private KeybindInputHandler input;

	/// <summary>
	/// The main camera in this scene.
	/// </summary>
	private GameObject mainCamera;
	#endregion
	#endregion

	#region Functions
	#region Initialization
	/// <summary>
	/// Gets the main camera of this scene, initializes values, and gets components.
	/// </summary>
	/// 
	bool healing;
	public Slider healthBar;
	public Text hpTxt;
	public AudioSource aSource;
	public AudioClip heal;
	private void Awake()
	{
		normalSpeedCapSquared = normalSpeedCap * normalSpeedCap;
		fastSpeedCapSquared = fastSpeedCap * fastSpeedCap;
		dashSpeedCapSqr = dashSpeedCap * dashSpeedCap;
		exitDashSpeedClampSquared = exitDashSpeedClamp * exitDashSpeedClamp;

		InitializeCameras();

		rb = GetComponent<Rigidbody>();
		input = GetComponent<KeybindInputHandler>();
		gun = GetComponentInChildren<Gun>();

		abilities = GetComponents<AbilityAction>();

		AddAbilities();
	}

	/// <summary>
	/// Assigns animations IDs.
	/// </summary>
	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		healing = false;
		health = 100;
		healthBar.value = health;
		hpTxt.text = "HP: " + healthBar.value;
		AssignAnimationIDs();
	}

	private void InitializeCameras()
    {
		mainCamera = Camera.main.gameObject;
		camBrain = mainCamera.GetComponent<CinemachineBrain>();

		if(PlayerPrefs.HasKey("X Sens"))
        {
			xSens = PlayerPrefs.GetFloat("X Sens");
			ySens = PlayerPrefs.GetFloat("Y Sens");

			adsAimMod = PlayerPrefs.GetFloat("Sens ADS");

			XFastSens = PlayerPrefs.GetFloat("X Sens Fast");
			YFastSens = PlayerPrefs.GetFloat("Y Sens Fast");
		}
	}

	private void AddAbilities()
    {
		GravityPull gp = gameObject.AddComponent<GravityPull>();
		FreezeEnemy fe = gameObject.AddComponent<FreezeEnemy>();
		Shield s = gameObject.AddComponent<Shield>();

		GameObject abilityManager = transform.Find("Ability Manager").gameObject;

		gp.Ability = abilityManager.GetComponent<GravityPull>().Ability;
		fe.Ability = abilityManager.GetComponent<FreezeEnemy>().Ability;
		s.Ability = abilityManager.GetComponent<Shield>().Ability;

		abilities = GetComponents<AbilityAction>();
	}

	/// <summary>
	/// Assigns animations IDs.
	/// </summary>
	private void AssignAnimationIDs()
	{
		animIDSpeed = Animator.StringToHash(animSpeedName);
		animIDMotionSpeed = Animator.StringToHash(animMotionSpeedName);
	}
	#endregion

	#region Actions
	#region Update Calls
	/// <summary>
	/// Calls movement functions every frame.
	/// </summary>
	private void FixedUpdate()
	{
		if(currentMoveState != moveState.dash && currentMoveState != moveState.nomovecasting && !input.ShouldSlowDown)
		Move();

        if (input.ShouldSlowDown)
        {
			SlowDown();
        }

		if (currentMoveState != moveState.fast)
        {
			RotatePlayerDuringNormalMove();
			RotateNormalCamera();
		}

        if (currentMoveState.Equals(moveState.dash))
        {
			DashRoutine();
        }
	}
	#endregion

	#region Movement
	private void Move()
	{
        if (input.MoveFast)
        {
			if(currentMoveState == moveState.normal || currentMoveState == moveState.ADS)
            {
				ChangeActive(moveState.fast, fastCam);

				// Sets camera so that transition feels good
				Quaternion oldNormalADSRot = cinemachineNormalADSTarget.rotation;
				cinemachineCameraTarget.rotation = mainCamera.transform.rotation;
				cinemachineNormalADSTarget.rotation = oldNormalADSRot;
			}

			if(currentMoveState == moveState.fast)
            {
				FastMove();
			}
			else
            {
				input.MoveFast = false;
			}
		}
        else
        {
			if(currentMoveState == moveState.fast)
            {
				ChangeActive(moveState.normal, normalCam);
				cinemachineTargetXRot = 0;
				cinemachineTargetYRot = 0;
				visuals.transform.up = mainCamera.transform.up;
			}

			NormalMove();
        }
	}

	private void SlowDown()
    {
		rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * slowDownRate);
    }

    #region Normal Move
    private void NormalMove()
    {
		MoveHorizontally();

		if (input.MoveVertical != 0)
		{
			MoveVertically();
		}

		ClampVeloctiy(normalSpeedCap, normalSpeedCapSquared);
	}

    #region Horizontal Movement
	private void MoveHorizontally()
    {
		// normalise input direction
		Vector2 inputDirection = new Vector3(input.Move.x, input.Move.y).normalized;

		// movement in directions
		Vector3 forwardForce = inputDirection.y * visuals.transform.forward.normalized * Time.fixedDeltaTime * speedChangeRate;
		Vector3 sidewaysForce = inputDirection.x * visuals.transform.right.normalized * Time.fixedDeltaTime * speedChangeRate;

		rb.AddForce(forwardForce + sidewaysForce);
	}
    #endregion

    #region Vertical Movement
    /// <summary>
    /// Moves the player up and down on the y axis.
    /// </summary>
    private void MoveVertically()
	{
		rb.AddForce(input.MoveVertical * verticalSpeedChangeRate * visuals.transform.up.normalized * Time.fixedDeltaTime);
	}
	#endregion

	private void ClampVeloctiy(float speedCap, float speedCapSqr)
    {
		if(rb.velocity.sqrMagnitude > speedCapSqr)
        {
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, speedCap);
		}
    }

	private void RotatePlayerDuringNormalMove()
    {
		Quaternion targetRotation = cinemachineCameraTarget.rotation * Quaternion.Euler(0, cinemachineTargetXRot, 0);
		visuals.rotation = Quaternion.Lerp(visuals.rotation, targetRotation, rotationSmoothTime * Time.fixedDeltaTime);
	}
    #endregion

    #region Fast Move
	private void FastMove()
    {
		RotateFastCamera();

		// Visuals
		RotateSpeedLines();
		RotateFastCharacter();

		FastMovement();

		ClampVeloctiy(fastSpeedCap, fastSpeedCapSquared);
	}

    #region Visuals Rotation
    /// <summary>
    /// Calculates character rotation.
    /// </summary>
    private void RotateFastCharacter()
    {
		Quaternion targetRotation = cinemachineCameraTarget.transform.rotation * Quaternion.Euler(70, 0, 0);
		targetRotation *= Quaternion.Euler(new Vector3(0, 0, input.Move.x * -30));
		targetRotation *= Quaternion.Euler(new Vector3(input.Move.y * -30, 0, 0));

		visuals.transform.rotation = Quaternion.Lerp(visuals.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
	}

	private void RotateSpeedLines()
    {
		Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 90 + input.Move.x * speedLinesSidewaysRotation, 0)) * Quaternion.Euler(new Vector3(0, 0, input.Move.y * -speedLinesVerticalRotation));

		/*
		speedLines.transform.localRotation = Quaternion.Euler(new Vector3(0, 90 + input.Move.x * speedLinesSidewaysRotation, 0));
		speedLines.transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, input.Move.y * -speedLinesVerticalRotation));
		*/

		speedLines.transform.localRotation = Quaternion.Lerp(speedLines.transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

		if (input.Move != Vector2.zero)
		{
			speedLines.transform.localPosition = new Vector3(input.Move.x * xPositionalOffset, input.Move.y * yPositionalOffset, 10.35f);
		}
		else
		{
			speedLines.transform.localPosition = new Vector3(0, 0, 10.35f);
		}
	}
    #endregion

    private void FastMovement()
    {
		Vector3 forwardForce = mainCamera.transform.forward * Time.fixedDeltaTime * forwardAcceleration;
		Vector3 sidewaysForce = mainCamera.transform.right * Time.fixedDeltaTime * strafeAcceleration * input.Move.x;
		Vector3 verticalForce = mainCamera.transform.up * Time.fixedDeltaTime * verticalAcceleration * input.Move.y;
		rb.AddForce(forwardForce + sidewaysForce + verticalForce);
	}
	#endregion

	#region Dash
	/// <summary>
	/// Dashes in a selected direction if allowed to.
	/// </summary>
	/// <param name="dashDir"></param>
	public void Dash(Vector3 dashDir)
	{
		if ((currentMoveState == moveState.normal || currentMoveState.Equals(moveState.ADS)) && canDash)
        {
            if (currentMoveState.Equals(moveState.ADS))
            {
				ChangeActive(moveState.normal, normalCam);
            }

			currentMoveState = moveState.dash;
			canDash = false;
			startTime = Time.time;

			DashDirCheck(ref dashDir);
			this.dashDir = dashDir;
		}
    }

	private Vector3 dashDir;

	/// <summary>
	/// Checks if the player should stop dashing.
	/// </summary>
	private void DashRoutine()
    {
		rb.AddForce(dashDir * dashSpeedAcceleration);
		ClampVeloctiy(dashSpeedCap, dashSpeedCapSqr);

		if (Time.time > startTime + dashTime)
		{
			currentMoveState = moveState.normal;
			ClampVeloctiy(exitDashSpeedClamp, exitDashSpeedClampSquared);
			StartCoroutine(DashStagger());
			return;
		}
	}

	/// <summary>
	/// Finds the direction the player should dash in.
	/// </summary>
	/// <param name="dashDir">The direction the player should dash in.</param>
	private void DashDirCheck(ref Vector3 dashDir)
    {
		if (dashDir.x != 0)
		{
			dashDir = visuals.transform.right.normalized * dashDir.x;
		}
		else if (dashDir.y != 0)
		{
			dashDir = visuals.transform.up.normalized * dashDir.y;
		}
		else
		{
			dashDir = visuals.transform.forward.normalized * dashDir.z;
		}
	}

	/// <summary>
	/// Adds a wait time between dashes.
	/// </summary>
	/// <returns></returns>
	private IEnumerator DashStagger()
    {
		yield return new WaitForSeconds(dashStaggerTime);

		canDash = true;
	}
    #endregion
    #endregion

    #region Shoot
    /// <summary>
    /// Shoots the player's gun.
    /// </summary>
    /// <param name="shouldShoot">Holds true if the player should shoot.</param>
    public void Shoot(bool shouldShoot)
    {
		if (canShoot)
			gun.Shoot(shouldShoot);
		else
			gun.Shoot(false);
	}

	public void ADS(bool shouldADS)
    {
        if (shouldADS && currentMoveState == moveState.normal)
        {
			ChangeActive(moveState.ADS, adsCam);
		}
        else if(currentMoveState == moveState.ADS)
        {
			ChangeActive(moveState.normal, normalCam);
		}
    }
    #endregion

    #region Abilities
	/// <summary>
	/// Activates the alt click ability.
	/// </summary>
    public void ThreeAbility()
    {
		if(currentMoveState == moveState.normal || currentMoveState == moveState.ADS)
        {
			Ability ability = null;

			if (abilities.Length > 2 && abilities[2].TriggerAbility(ref ability))
			{
				StartCoroutine(CastMoveHandler(ability));
			}
		}
    }

	/// <summary>
	/// Activates the e click ability.
	/// </summary>
	public void TwoAbility()
    {
		if (currentMoveState == moveState.normal || currentMoveState == moveState.ADS)
		{
			Ability ability = null;

			if (abilities.Length > 1 &&  abilities[1].TriggerAbility(ref ability))
			{
				StartCoroutine(CastMoveHandler(ability));
			}
		}
    }

	/// <summary>
	/// Activatest the q click ability.
	/// </summary>
	public void OneAbility()
    {
		if (currentMoveState == moveState.normal || currentMoveState == moveState.ADS)
		{
			Ability ability = null;

			if (abilities.Length > 0 && abilities[0].TriggerAbility(ref ability))
			{
				CastMoveHandlerRef = StartCoroutine(CastMoveHandler(ability));
			}
		}
    }

	public void StopCasting()
    {
		StopCoroutine(CastMoveHandlerRef);
		currentMoveState = moveState.normal;
		canShoot = true;
	}

	private Coroutine CastMoveHandlerRef;
	private IEnumerator CastMoveHandler(Ability ability)
    {
		if(ability.CastStartupTime + ability.CastDuration + ability.UncastTime == 0)
        {
			yield break;
        }

		canShoot = ability.CanShootDuringCast;
		gun.Shoot(false);

		currentMoveState = ability.MovementDuringCastStartup ? moveState.casting : moveState.nomovecasting;
		yield return new WaitForSeconds(ability.CastStartupTime);

		currentMoveState = ability.MoveDuringCast ? moveState.casting : moveState.nomovecasting;
		yield return new WaitForSeconds(ability.CastDuration);

		currentMoveState = ability.MovementDuringUncast ? moveState.casting : moveState.nomovecasting;
		yield return new WaitForSeconds(ability.UncastTime);
		currentMoveState = moveState.normal;
		canShoot = true;

	}
    #endregion

    #region Health
	/// <summary>
	/// Allows the players health to be increase/decreased.
	/// </summary>
	/// <param name="healthMod">Amount of health added to the players current total. (If damage use negative number)</param>
	public void UpdateHealth(int healthMod)
    {
		health += healthMod;

		if(health <= 0)
        {
			PlayerDeath();
        }
		else if(health > startingHealth)
        {
			health = startingHealth;
        }

		UpdateHealthBar();
    }

	public void UpdateHealthBar()
    {
		healthBar.value = health;
		hpTxt.text = "HP: " + health;

	}

	private void PlayerDeath()
    {
		SceneManager.LoadScene("Level");
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "heal" && healthBar.value < 100)
		{
			healing = true;
			aSource.clip = heal;
			aSource.Play();
			StartCoroutine(HealPlayer());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "heal")
		{
			healing = false;
		}
	}

	IEnumerator HealPlayer()
	{
		yield return new WaitForSeconds(1f);
		healthBar.value += 2;
		hpTxt.text = "HP: " + healthBar.value;
	}
	#endregion

	#region Change Move States
	private void ChangeActive(moveState newMoveState, CinemachineVirtualCamera newCam)
	{
		MoveFast.Invoke(newMoveState == moveState.fast);

		camBrain.ActiveVirtualCamera.Priority = 0;
		newCam.Priority = 1;
		currentMoveState = newMoveState;
	}
	#endregion

	#region Camera Rotation
	private void RotateFastCamera()
	{
		float yRotation = 0;
		yRotation -= input.Look.y * yFastSens * Time.fixedDeltaTime;

		cinemachineCameraTarget.transform.Rotate(new Vector3(-yRotation, 0, 0));
		cinemachineCameraTarget.transform.RotateAround(cinemachineCameraTarget.transform.position, -cinemachineCameraTarget.transform.up, -input.Look.x * xFastSens * Time.fixedDeltaTime);
	}

	/// <summary>
	/// Rotates the objects that the cinemachine camera is following.
	/// </summary>
	private void RotateNormalCamera()
	{
		float adsmod = currentMoveState.Equals(moveState.ADS) ? adsAimMod : 1;

		// Updates the target look values
		if (input.Look.sqrMagnitude >= lookAmountThreshold)
		{
			cinemachineTargetXRot += input.Look.x * Time.fixedDeltaTime * xSens * adsmod;
			cinemachineTargetYRot += input.Look.y * Time.fixedDeltaTime * ySens * adsmod;
		}

		// clamp our rotations so our values are limited 360 degrees
		cinemachineTargetXRot = ClampAngle(cinemachineTargetXRot, float.MinValue, float.MaxValue);
		cinemachineTargetYRot = ClampAngle(cinemachineTargetYRot, bottomClamp, topClamp);

		// Updates cinemachine follow target rotation (essentially rotates the camera)
		cinemachineNormalADSTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetYRot, cinemachineTargetXRot, 0.0f);
	}

	/// <summary>
	/// Clamps the camera angle between given values.
	/// </summary>
	/// <param name="targetAngle">The current targeted angle.</param>
	/// <param name="angleMin">The current minimum angle for this axis.</param>
	/// <param name="angleMax">The current maximum angle for this axis.</param>
	/// <returns></returns>
	private float ClampAngle(float targetAngle, float angleMin, float angleMax)
	{
		if (targetAngle < -360f) targetAngle += 360f;
		if (targetAngle > 360f) targetAngle -= 360f;
		return Mathf.Clamp(targetAngle, angleMin, angleMax);
	}
    #endregion
    #endregion
    #endregion
}