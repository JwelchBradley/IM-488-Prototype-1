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
		nomovecasting
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

	[Tooltip("The camera used for fast movement")]
	[SerializeField]
	private CinemachineVirtualCamera fastCam;

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
	private void Update()
	{
		MoveVertically();

		if(currentMoveState != moveState.dash && currentMoveState != moveState.nomovecasting)
		Move();
	}

	/// <summary>
	/// Updates the camera after all movements have been made.
	/// </summary>
	private void LateUpdate()
	{
		//CameraRotation();
	}
	#endregion

	#region Movement
	private void Move()
	{
        if (input.MoveFast)
        {
			if(currentMoveState == moveState.normal)
            {
				ChangeBetweenMoveStates(moveState.fast, 1, true);
			}

			FastMove();
        }
        else
        {
			if (currentMoveState == moveState.fast)
			{
				ChangeBetweenMoveStates(moveState.normal, -1, false);
			}

			NormalMove();
        }
	}

	private void ChangeBetweenMoveStates(moveState newMoveState, int camPriorityMod, bool isFast)
    {
		MoveFast.Invoke(isFast);
		currentMoveState = newMoveState;
		fastCam.Priority = normalCam.Priority + camPriorityMod;
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
			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}


		Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

		// move the player
		Vector3 verticalMovement = new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime;
		Vector3 horizontalMovement = targetDirection.normalized * (currentSpeed * Time.deltaTime);
		controller.Move(horizontalMovement + verticalMovement);
	}

	float activeForwardSpeed = 0;
	float activeStrafeSpeed = 0;
	float activeHoverSpeed = 0;
	private void FastMove()
    {
		float forwardSpeed = 10f, strafeSpeed = 7.5f, hoverSpeed = 10;
		float forwardAcceleration = 2.5f, strafeAcceleration = 2.0f, hoverAcceleration = 2.0f; 
		Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.forward)*Quaternion.Euler(90, 0, 0);
		transform.rotation = targetRotation;
		//transform.rotation = Mathf.Lerp(transform.rotation, targetRotation, ;

		activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, (1 * forwardSpeed), forwardAcceleration * Time.deltaTime);

		activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, (input.Move.x * strafeSpeed), strafeAcceleration * Time.deltaTime);

		activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, (input.Move.y * hoverSpeed), hoverAcceleration * Time.deltaTime);

		Vector3 forwardMovement = Camera.main.transform.forward * activeForwardSpeed * Time.deltaTime;
		Vector3 strafeMovement = Camera.main.transform.right * activeStrafeSpeed * Time.deltaTime;
		Vector3 hoverMovement = Camera.main.transform.up * activeHoverSpeed * Time.deltaTime;

		controller.Move(forwardMovement + strafeMovement + hoverMovement);
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
			controller.Move(targetDirection.normalized * dashSpeed * Time.deltaTime);

			yield return new WaitForEndOfFrame();
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
	public void Shoot(bool shouldShoot)
    {
		gun.Shoot(shouldShoot);
	}
    #endregion

    #region Abilities
	/// <summary>
	/// Activates the alt click ability.
	/// </summary>
    public void AltAbility()
    {
		Ability ability = null;

		if (abilities[0].TriggerAbility(ref ability))
		{
			StartCoroutine(CastMoveHandler(ability));
			//SetCastMoveState(ability.MoveDuringCast);
		}
    }

	/// <summary>
	/// Activates the e click ability.
	/// </summary>
	public void EAbility()
    {
		Ability ability = null;

		if(abilities[1].TriggerAbility(ref ability))
        {
			StartCoroutine(CastMoveHandler(ability));
			//SetCastMoveState(ability.MoveDuringCast);
		}
    }

	/// <summary>
	/// Activatest the q click ability.
	/// </summary>
	public void QAbility()
    {
		Ability ability = null;

		if (abilities[2].TriggerAbility(ref ability))
        {
			StartCoroutine(CastMoveHandler(ability));
			//SetCastMoveState(ability.MoveDuringCast);
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
	#endregion
	#endregion
}