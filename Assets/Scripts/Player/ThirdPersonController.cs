/*****************************************************************************
// File Name :         ThirdPersonController.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the movement of the player.
*****************************************************************************/
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

[RequireComponent(typeof(CharacterController))]
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

	#region Flat Movement
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

	[Header("Horizontal Movement")]
	[Header("-------------Movement-------------")]
	[Tooltip("Move speed of the character in m/s")]
	[Range(0, 50)]
	[SerializeField]
	private float normalSpeed = 2.0f;

	[Tooltip("Sprint speed of the character in m/s")]
	[Range(0, 100)]
	[SerializeField]
	private float fastSpeed = 5.335f;

	[Tooltip("Acceleration and deceleration of the character ground movement")]
	[Range(0.0f, 50.0f)]
	[SerializeField]
	private float speedChangeRate = 10.0f;
	#endregion

	#region Vertical Movement
	[Header("Vertical Movement")]
	[Tooltip("How fast the player can move vertically")]
	[Range(0.0f, 50.0f)]
	[SerializeField]
	private float verticalSpeed = 3.0f;

	/// <summary>
	/// How fast the player is currently moving in the vertical axis.
	/// </summary>
    private float verticalVelocity;

	[Tooltip("How fast the player changes their vertical speed")]
	[Range(0.0f, 3.0f)]
	[SerializeField]
	private float verticalSpeedChangeRate = 0.1f;

	[Tooltip("How fast the player comes to a vertical stop")]
	[Range(2.0f, 100.0f)]
	[SerializeField]
	private float verticalStopSpeed = 5.0f;
	#endregion

	#region FastMovement
	[Header("Fast move")]
	[Tooltip("How fast the player can move left or right during fast moves")]
	[SerializeField] float strafeSpeed = 7.5f;
	[Tooltip("How fast the player can move up or down during fast moves")]
	[SerializeField] float hoverSpeed = 10;
	[Tooltip("How much the camera looks ahead while strafing")]
	[SerializeField] private float strafeLookAhead = 1.0f;
	[Tooltip("How much the camera looks ahead while hovering")]
	[SerializeField] private float hoverLookAhead = 1.0f;
	#endregion

	#region Dash
	[Header("Dash")]
	[Tooltip("How fast the player is while dashing")]
	[SerializeField]
	private float dashSpeed = 10.0f;

	[Tooltip("How long the dash lasts")]
	[Range(0.0f, 1.0f)]
	[SerializeField]
	private float dashTime = 0.5f;

	[Tooltip("How long before the player can dash again")]
	[Range(0.0f, 2.0f)]
	[SerializeField]
	private float dashStaggerTime = 0.5f;

	/// <summary>
	/// Holds true if the player can dash.
	/// </summary>
	private bool canDash = true;
    #endregion

    #region Character Rotation
    [Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)]
	[SerializeField]
	private float rotationSmoothTime = 0.12f;

	/// <summary>
	/// How fast the player's character is rotating.
	/// </summary>
	private float rotationVelocity;

	/// <summary>
	/// The current target rotation of the player's character.
	/// </summary>
	private float targetRotation = 0.0f;
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
	private int health = 0;
    #endregion

    #region Player Grounding
	/*
    [Header("Player Grounded")]
	/// <summary>
	/// Holds reference to if the player is grounded. NOT the built in character controller grounded.
	/// </summary>
	private bool grounded = true;

	[Tooltip("How far beneath the player is checked for grounds")]
	[SerializeField]
	[Range(-0.5f, 0.0f)]
	private float groundedOffset = -0.14f;

	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	[SerializeField]
	[Range(0.0f, 2.0f)]
	private float groundedRadius = 0.28f;

	[Tooltip("What layers the character uses as ground")]
	[SerializeField]
	private LayerMask groundLayers;*/

	#region Time out deltas
	/// <summary>
	/// Timeout delta for how long after landing before the player can jump again.
	/// </summary>
	private float jumpTimeoutDelta;

	/// <summary>
	/// Timeout delta for how long after falling off of an object before the player has the fall animation played.
	/// </summary>
	private float fallTimeoutDelta;
	#endregion
	#endregion
	#endregion

	#region Cinemachine
	[Header("Cinemachine")]
	#region Aim
	[Tooltip("The camera used for normal movement")]
	[SerializeField]
	private CinemachineVirtualCamera normalCam;

	private CinemachinePOV normalCamPOV;

	public CinemachinePOV NormalCamPOV
    {
		get => normalCamPOV;

	}

	[Tooltip("The camera used for fast movement")]
	[SerializeField]
	private CinemachineVirtualCamera fastCam;

	private CinemachinePOV fastCamPOV;

	public CinemachinePOV FastCamPOV
	{
		get => fastCamPOV;

	}

	[Tooltip("The camera used for ADS")]
	[SerializeField]
	private CinemachineVirtualCamera adsCam;

	private CinemachinePOV adsCamPOV;

	public CinemachinePOV AdsCamPOV
	{
		get => adsCamPOV;

	}

	private CinemachineVirtualCamera oldCam;

	private CinemachinePOV oldCamPOV;

	[SerializeField]
	private GameObject cinemachineCameraTarget;

	/// <summary>
	/// The current target rotation of the cinemachine follow object.
	/// </summary>
	private float cinemachineTargetXRot;

	/// <summary>
	/// How sensitive the camera is in the x-axis.
	/// </summary>
	private float xSens = 10;

	/// <summary>
	/// How sensitive the camera is in the y-axis.
	/// </summary>
	private float cinemachineTargetYRot;

	/// <summary>
	/// How sensitive the camera is in the y-axis.
	/// </summary>
	private float ySens = 10;
	#endregion
	#endregion

	#region Animation IDs
	[Header("Animation")]
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

	#region Components
	/// <summary>
	/// The animator component of this character.
	/// </summary>
	private Animator animator;

	/// <summary>
	/// The character controller of the player.
	/// </summary>
	private CharacterController controller;

	/// <summary>
	/// The input manager for the player.
	/// </summary>
	private KeybindInputHandler input;

	/// <summary>
	/// The main camera in this scene.
	/// </summary>
	private GameObject mainCamera;

	/// <summary>
	/// The players gun.
	/// </summary>
	private Gun gun;

	private AbilityAction[] abilities;

	public AbilityAction[] Abilities
    {
		get => abilities;
    }

	[SerializeField]
	private Ability[] abilityData;

	public class MoveFastEvent : UnityEvent<bool> { }

	public MoveFastEvent MoveFast = new MoveFastEvent();
	#endregion
	#endregion

	#region Functions
	#region Initialization
	/// <summary>
	/// Gets the main camera of this scene, initializes values, and gets components.
	/// </summary>
	private void Awake()
	{
		mainCamera = Camera.main.gameObject;

		normalCamPOV = normalCam.GetCinemachineComponent<CinemachinePOV>();
		fastCamPOV = fastCam.GetCinemachineComponent<CinemachinePOV>();
		adsCamPOV = adsCam.GetCinemachineComponent<CinemachinePOV>();
		oldCam = normalCam;
		oldCamPOV = normalCamPOV;

		InitializeCameras();

		controller = GetComponent<CharacterController>();
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

		AssignAnimationIDs();
	}

	private void InitializeCameras()
    {
		normalCamPOV.m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("X Sens");
		normalCamPOV.m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Y Sens");

		Debug.Log(PlayerPrefs.GetFloat("X Sens Fast"));
		Debug.Log(PlayerPrefs.GetFloat("Y Sens Fast"));

		fastCamPOV.m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("X Sens Fast")/10;
		fastCamPOV.m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Y Sens Fast")/10;

		adsCamPOV.m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("X Sens ADS");
		adsCamPOV.m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetFloat("Y Sens ADS");

		fastCamPOV.enabled = false;
		adsCamPOV.enabled = false;
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
		MoveVertically();

		if(currentMoveState != moveState.dash && currentMoveState != moveState.nomovecasting)
		Move();

		if (currentMoveState != moveState.fast)
			RotatePlayerDuringMove();
	}


	/// <summary>
	/// Updates the camera after all movements have been made.
	/// </summary>
	private void LateUpdate()
	{

	}
	#endregion

	#region Movement
	private void Move()
	{
        if (input.MoveFast)
        {
			if(currentMoveState == moveState.normal || currentMoveState == moveState.ADS)
            {
				ChangeBetweenMoveStates(moveState.fast, fastCamPOV, fastCam);
				/*
				currentMoveState = moveState.fast;
				fastCam.Priority = oldCam.Priority + 1;*/
			}

			if(currentMoveState == moveState.fast)
			FastMove();
			else
            {
				input.MoveFast = false;
			}
		}
        else
        {
			if (currentMoveState == moveState.fast)
			{
				ChangeBetweenMoveStates(moveState.normal, normalCamPOV, normalCam);
			}

			NormalMove();
        }
	}

	private void NormalMove()
    {
		float targetSpeed = input.MoveFast ? fastSpeed : normalSpeed;

		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (input.Move == Vector2.zero) targetSpeed = 0.0f;

		// a reference to the players current horizontal velocity
		float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

		float speedOffset = 0.1f;

		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			// creates curved result rather than a linear one giving a more organic speed change
			// note T in Lerp is clamped, so we don't need to clamp our speed
			currentSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * input.Move.magnitude, Time.deltaTime * speedChangeRate);

			// round speed to 3 decimal places
			currentSpeed = Mathf.Round(currentSpeed * 1000f) / 1000f;
		}
		else
		{
			currentSpeed = targetSpeed;
		}

		// normalise input direction
		Vector3 inputDirection = new Vector3(input.Move.x, 0.0f, input.Move.y).normalized;

		// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is a move input rotate player when the player is moving
		if (input.Move != Vector2.zero)
		{
			CalculateTargetRotation(inputDirection);
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

			// rotate to face input direction relative to camera position
			//transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}


		Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

		// move the player
		Vector3 verticalMovement = new Vector3(0.0f, verticalVelocity, 0.0f) * Time.fixedDeltaTime;
		Vector3 horizontalMovement = targetDirection.normalized * (currentSpeed * Time.fixedDeltaTime);
		controller.Move(horizontalMovement + verticalMovement);
	}

	private void RotatePlayerDuringMove()
    {
			float targetAngle = mainCamera.transform.eulerAngles.y;
			Quaternion rot = Quaternion.Euler(0, targetAngle, 0);;
			transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
	}

	float rotationSpeed = 10;
	float activeForwardSpeed = 0;
	float activeStrafeSpeed = 0;
	float activeHoverSpeed = 0;

	private void FastMove()
    {
		//RotateCamera();

		
		float forwardAcceleration = 2.5f, strafeAcceleration = 2.0f, hoverAcceleration = 2.0f; 
		Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward)*Quaternion.Euler(90, 0, 0);
		
		if(fastCamPOV.m_VerticalAxis.Value >= 90 || fastCamPOV.m_VerticalAxis.Value <= -90)
        {
			targetRotation *= Quaternion.Euler(0, 180, 0);
			fastCamPOV.m_HorizontalAxis.m_InvertInput = true;
        }
        else
        {
			fastCamPOV.m_HorizontalAxis.m_InvertInput = false;
		}

		
		//transform.rotation = Mathf.Lerp(transform.rotation, targetRotation, ;

		activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, fastSpeed, forwardAcceleration * Time.fixedDeltaTime);
		
		activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, (input.Move.x * strafeSpeed), strafeAcceleration * Time.fixedDeltaTime);
		targetRotation *= Quaternion.Euler(new Vector3(0, 0, input.Move.x*-30));
		activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, (input.Move.y * hoverSpeed), hoverAcceleration * Time.fixedDeltaTime);
		targetRotation *= Quaternion.Euler(new Vector3(input.Move.y * -30, 0, 0));

		Vector3 forwardMovement = Camera.main.transform.forward * activeForwardSpeed * Time.fixedDeltaTime;
		Vector3 strafeMovement = Camera.main.transform.right * activeStrafeSpeed * Time.fixedDeltaTime;
		Vector3 hoverMovement = Camera.main.transform.up * activeHoverSpeed * Time.fixedDeltaTime;

		controller.Move(forwardMovement + strafeMovement + hoverMovement);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

		/*
		CinemachineFramingTransposer cft = fastCam.GetCinemachineComponent<CinemachineFramingTransposer>();
		cft.m_ScreenX = 0.5f + -input.Move.x * strafeLookAhead;
		cft.m_ScreenY = 0.7f + input.Move.y * hoverLookAhead;*/
		//transform.rotation = targetRotation;
	}

	private void RotateCamera()
    {
		Vector2 mouseDistance;
		Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2;
		mouseDistance.x = (input.Look.x - screenCenter.x) / screenCenter.y;
		mouseDistance.y = (input.Look.y - screenCenter.y) / screenCenter.y;

		mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1);

		//Camera.main.transform.Rotate(-mouseDistance.y*Time.deltaTime, mouseDistance.x *Time.deltaTime);

		/*
		float lookAmountThreshold = .01f;

		// Updates the target look values
		if (input.Look.sqrMagnitude >= lookAmountThreshold)
		{
			cinemachineTargetXRot += input.Look.x * Time.deltaTime * xSens;
			cinemachineTargetYRot += input.Look.y * Time.deltaTime * ySens;
		}

		// Updates cinemachine follow target rotation (essentially rotates the camera)
		cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetYRot, cinemachineTargetXRot, 0.0f);
		mainCamera.transform.rotation = Quaternion.Euler(cinemachineTargetYRot, cinemachineTargetXRot, 0.0f);
	*/
	}

	#region Dash
	public void Dash(Vector3 dashDir)
    {
		if(currentMoveState == moveState.normal && canDash)
        {
			currentMoveState = moveState.dash;
			canDash = false;
			StartCoroutine(DashRoutine(dashDir));
		}
    }

	private IEnumerator DashRoutine(Vector3 dashDir)
    {
		float startTime = Time.time;

		CalculateTargetRotation(dashDir);

		while (Time.time < startTime + dashTime)
        {
			Vector3 targetDirection = dashDir;
			if (dashDir.y == 0)
            {
				targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
			}
			
			//targetDirection.normalized
			controller.Move(targetDirection.normalized * dashSpeed * Time.fixedDeltaTime);

			yield return new WaitForFixedUpdate();
        }

		currentMoveState = moveState.normal;

		yield return new WaitForSeconds(dashStaggerTime);

		canDash = true;
    }

    #endregion

    /// <summary>
    /// Moves the player up and down on the y axis.
    /// </summary>
    private void MoveVertically()
    {
		if (input.MoveVertical == 0)
		{
			verticalVelocity /= verticalStopSpeed;
		}
        else
		{
			verticalVelocity += verticalSpeedChangeRate * input.MoveVertical;
			verticalVelocity = Mathf.Clamp(verticalVelocity, -verticalSpeed, verticalSpeed);
		}
    }

	private void CalculateTargetRotation(Vector3 inputDirection)
    {
		targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
	}
    #endregion

    #region Shoot
	/// <summary>
	/// Shoots the player's gun.
	/// </summary>
	/// <param name="shouldShoot">Holds true if the player should shoot.</param>
	public void Shoot(bool shouldShoot)
    {
		gun.Shoot(shouldShoot);
	}

	public void ADS(bool shouldADS)
    {
        if (shouldADS && currentMoveState == moveState.normal)
        {
			Debug.Log(currentMoveState);
			currentMoveState = moveState.ADS;
			ChangeBetweenMoveStates(moveState.ADS, adsCamPOV, adsCam);
		}
        else if(currentMoveState == moveState.ADS)
        {
			currentMoveState = moveState.normal;
			ChangeBetweenMoveStates(moveState.normal, normalCamPOV, normalCam);
		}
    }
    #endregion

    #region Abilities
	/// <summary>
	/// Activates the alt click ability.
	/// </summary>
    public void XAbility()
    {
		if(currentMoveState == moveState.normal || currentMoveState == moveState.ADS)
        {
			Ability ability = null;

			if (abilities.Length > 2 && abilities[2].TriggerAbility(ref ability))
			{
				StartCoroutine(CastMoveHandler(ability));
				//SetCastMoveState(ability.MoveDuringCast);
			}
		}
    }

	/// <summary>
	/// Activates the e click ability.
	/// </summary>
	public void EAbility()
    {
		if (currentMoveState == moveState.normal || currentMoveState == moveState.ADS)
		{
			Ability ability = null;

			if (abilities[1].TriggerAbility(ref ability))
			{
				StartCoroutine(CastMoveHandler(ability));
				//SetCastMoveState(ability.MoveDuringCast);
			}
		}
    }

	/// <summary>
	/// Activatest the q click ability.
	/// </summary>
	public void QAbility()
    {
		if (currentMoveState == moveState.normal || currentMoveState == moveState.ADS)
		{
			Ability ability = null;

			if (abilities[0].TriggerAbility(ref ability))
			{
				StartCoroutine(CastMoveHandler(ability));
				//SetCastMoveState(ability.MoveDuringCast);
			}
		}
    }

	private void SetCastMoveState(bool canMove)
    {
        if (canMove)
        {
			currentMoveState = moveState.casting;
        }
        else
        {
			currentMoveState = moveState.nomovecasting;
        }
    }

	private IEnumerator CastMoveHandler(Ability ability)
    {
		currentMoveState = ability.MovementDuringCastStartup ? moveState.casting : moveState.nomovecasting;
		yield return new WaitForSeconds(ability.CastStartupTime);

		currentMoveState = ability.MoveDuringCast ? moveState.casting : moveState.nomovecasting;
		yield return new WaitForSeconds(ability.CastDuration);

		currentMoveState = ability.MovementDuringUncast ? moveState.casting : moveState.nomovecasting;
		yield return new WaitForSeconds(ability.UncastTime);
		currentMoveState = moveState.normal;
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
    }

	private void PlayerDeath()
    {

    }
    #endregion

    #region Change Move States
    private void ChangeBetweenMoveStates(moveState newMoveState, CinemachinePOV currentCamPOV, CinemachineVirtualCamera currentCam)
	{
		MoveFast.Invoke(newMoveState == moveState.fast);
		currentMoveState = newMoveState;
		currentCam.Priority = oldCam.Priority + 1;
		currentCamPOV.m_HorizontalAxis.Value = oldCamPOV.m_HorizontalAxis.Value;
		currentCamPOV.m_VerticalAxis.Value = oldCamPOV.m_VerticalAxis.Value;

		currentCamPOV.enabled = true;
		oldCamPOV.enabled = false;

		oldCam = currentCam;
		oldCamPOV = currentCamPOV;
	}
    #endregion
    #endregion
    #endregion
}