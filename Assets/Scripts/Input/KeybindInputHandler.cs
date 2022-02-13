/*****************************************************************************
// File Name :         KeybindInputHandler.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles user inputs.
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindInputHandler : MonoBehaviour
{
    #region Variables
    #region Inputs
    #region Movement
    /// <summary>
    /// Normal 4 way movement values.
    /// </summary>
    private Vector2 move;

	/// <summary>
	/// Normal 4 way movement values.
	/// </summary>
	public Vector2 Move
    {
		get => move;
    }

	/// <summary>
	/// The movement in the vertical axis.
	/// </summary>
	private float moveVertical;

	/// <summary>
	/// The movement in the vertical axis.
	/// </summary>
	public float MoveVertical
    {
		get => moveVertical;
    }

	/// <summary>
	/// Holds true if the player wants to move fast.
	/// </summary>
	private bool moveFast;

	/// <summary>
	/// Holds true if the player wants to move fast.
	/// </summary>
	public bool MoveFast
	{
		get => moveFast;
        set
        {
			moveFast = value;
        }
	}

	private bool shouldSlowDown = false;

	public bool ShouldSlowDown
	{
		get => shouldSlowDown;
    }
    #endregion

    #region Aim
    /// <summary>
    /// The delta aim values.
    /// </summary>
    private Vector2 look;

	/// <summary>
	/// The delta aim values.
	/// </summary>
	public Vector2 Look
	{
		get => look;
	}
    #endregion
    #endregion

	/// <summary>
	/// The pause menu in this scene.
	/// </summary>
	private PauseMenuBehavior pmb;

	/// <summary>
	/// The player's controller.
	/// </summary>
	private ThirdPersonController tpc;

	[Tooltip("How long after pressing the key will it stay primed")]
	[SerializeField] private float dashTapHoldTime;
	#endregion

	#region Functions
	#region Initialization
	/// <summary>
	/// Initializes components.
	/// </summary>
	private void Awake()
    {
		pmb = GameObject.Find("Pause Menu Templates Canvas").GetComponent<PauseMenuBehavior>();
		tpc = GetComponent<ThirdPersonController>();
	}
	#endregion

	#region Input recievers
	#region Movement
	[SerializeField] InputActionReference inputAction;
	InputControl inputControl;
	Vector3 oldValue = Vector3.zero;

	/// <summary>
	/// Gets the input move values.
	/// </summary>
	/// <param name="value">Input move value.</param>
	public void OnMove(InputValue value)
	{
		Vector3 newValue = value.Get<Vector2>();

		// Finds if the key was released or pressed
		InputControl currentInputControl = inputAction.action.activeControl;
		bool release = false;
		CheckIfKeyReleased(ref release, currentInputControl);

        if (!release)
        {
			if (inputControl == null)
			{
				inputControl = currentInputControl;
				if (dashUpdateRef != null)
				{
					StopCoroutine(dashUpdateRef);
					ResetDashVariables();
				}
				dashUpdateRef = StartCoroutine(DashPrimerHoldNew());
			}
			else if(currentInputControl == inputControl)
			{
				Vector3 dashValue = new Vector3(newValue.x, 0, newValue.y);
				if (newValue == Vector3.zero && (currentActiveMovementInputs.Count == 2 || currentActiveMovementInputs.Count == 4))
					dashValue = new Vector3(oldValue.x * -1, 0, oldValue.y * -1);
				DashInput(dashValue);

				if (dashUpdateRef != null)
				{
					StopCoroutine(dashUpdateRef);
					ResetDashVariables();
				}
			}
            else
            {
				if (dashUpdateRef != null)
				{
					StopCoroutine(dashUpdateRef);
					ResetDashVariables();
				}
			}
		}

		oldValue = newValue;
		MoveInput(newValue);
	}

    #region Dash
    private List<InputControl> currentActiveMovementInputs = new List<InputControl>();
	private void CheckIfKeyReleased(ref bool released, InputControl currentInputControl)
    {
		if (currentActiveMovementInputs.Contains(currentInputControl))
        {
			released = true;
			currentActiveMovementInputs.Remove(currentInputControl);
		}
        else
        {
			currentActiveMovementInputs.Add(currentInputControl);
        }
    }

	private Coroutine dashUpdateRef;
	private IEnumerator DashPrimerHoldNew()
	{
		yield return new WaitForSeconds(dashTapHoldTime);
		ResetDashVariables();
	}

	private void ResetDashVariables()
    {
		inputControl = null;
		dashUpdateRef = null;
	}
	#endregion

	private float verticalDashPrimed = 0;
	private Coroutine verticalDashPrimedRef;
	public void OnMoveVertically(InputValue value)
    {
		float floatValue = value.Get<float>();

		if(floatValue != 0 && floatValue == verticalDashPrimed)
        {
			DashInput(new Vector3(0, floatValue, 0));
        }
        else if(floatValue != 0)
        {
			if(verticalDashPrimedRef != null)
            {
				StopCoroutine(verticalDashPrimedRef);
				verticalDashPrimedRef = null;
            }
			verticalDashPrimedRef = StartCoroutine(VerticalDashPrimerHolder());
			verticalDashPrimed = floatValue;
        }

		VerticalInput(floatValue);
    }

	private IEnumerator VerticalDashPrimerHolder()
    {
		yield return new WaitForSeconds(dashTapHoldTime);
		verticalDashPrimed = 0;
		verticalDashPrimedRef = null;
	}

    /// <summary>
    /// Gets the input move fast values.
    /// </summary>
    /// <param name="value">Input move fast value.</param>
    public void OnMoveFast(InputValue value)
	{
		MoveFastInput(value.isPressed);
	}
    #endregion

    #region Shoot
    /// <summary>
    /// Gets the input look values.
    /// </summary>
    /// <param name="value">Input look value.</param>
    public void OnLook(InputValue value)
	{
		LookInput(value.Get<Vector2>());
	}

	/// <summary>
	/// Gets the input shoot values.
	/// </summary>
	/// <param name="value">Input shoot value.</param>
	public void OnShoot(InputValue value)
    {
		ShootInput(value.isPressed);
    }
    #endregion

    /// <summary>
    /// Gets the input pause values.
    /// </summary>
    /// <param name="value">Input pause value.</param>
    public void OnPause()
    {
		pmb.PauseGame();
    }

    #region Abilities
    /// <summary>
    /// Gets the input alt ability values.
    /// </summary>
    /// <param name="value">Input alt ability value.</param>
    public void OnThreeAbility(InputValue value)
    {
		ThreeAbilityInput(value.isPressed);
    }

	/// <summary>
	/// Gets the input E ability values.
	/// </summary>
	/// <param name="value">Input E ability value.</param>
	public void OnTwoAbility(InputValue value)
    {
		TwoAbilityInput(value.isPressed);
	}

	/// <summary>
	/// Gets the input Q ability values.
	/// </summary>
	/// <param name="value">Input Q ability value.</param>
	public void OnOneAbility(InputValue value)
	{
		OneAbilityInput(value.isPressed);
	}

	public void OnADS(InputValue value)
    {
		OnADSInput(value.isPressed);
    }

	public void OnSlowDown(InputValue value)
    {
		OnSlowDownInput(value.isPressed);
    }
    #endregion
    #endregion

    #region Input Updaters
    #region Movement
    /// <summary>
    /// WASD movement values as a vector2.
    /// </summary>
    /// <param name="newMoveDirection">The WASD movement values.</param>
    public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	}

	/// <summary>
	/// Gets the dash input values.
	/// </summary>
	/// <param name="newDashDirection"></param>
	private void DashInput(Vector3 newDashDirection)
    {
		tpc.Dash(newDashDirection);
    }

	/// <summary>
	/// Holds float for up or down movement.
	/// </summary>
	/// <param name="newVerticalState">1 for up; -1 for down.</param>
	public void VerticalInput(float newVerticalState)
	{
		moveVertical = newVerticalState;
	}

	/// <summary>
	/// Calls for the player to move fast.
	/// </summary>
	/// <param name="newMoveFastState">Holds true if the player wants to move fast.</param>
	public void MoveFastInput(bool newMoveFastState)
	{
		moveFast = !moveFast;
		//moveFast = newMoveFastState;
	}

	private void OnSlowDownInput(bool shouldSlowDown)
    {
		this.shouldSlowDown = shouldSlowDown;
    }
	#endregion

	#region Shoot
	/// <summary>
	/// Calls for the player to shoot.
	/// </summary>
	/// <param name="shouldShoot">Holds true if the player should shoot.</param>
	private void ShootInput(bool shouldShoot)
	{
		if(Time.timeScale != 0)
		tpc.Shoot(shouldShoot);
	}

	private void OnADSInput(bool shouldADS)
    {
		tpc.ADS(shouldADS);
    }

	/// <summary>
	/// Gets the mouse delta movement since the last frame.
	/// </summary>
	/// <param name="newLookDirection">The delta mouse movement.</param>
	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}
	#endregion

	#region Abilities
	/// <summary>
	/// Calls for the ability in the alt click position to be used.
	/// </summary>
	/// <param name="shouldUseAbility">Holds true if the ability should be used.</param>
	private void ThreeAbilityInput(bool shouldUseAbility)
    {
		if(shouldUseAbility && Time.timeScale != 0)
		tpc.ThreeAbility();
    }

	/// <summary>
	/// Calls for the ability in the E position to be used.
	/// </summary>
	/// <param name="shouldUseAbility">Holds true if the ability should be used.</param>
	private void TwoAbilityInput(bool shouldUseAbility)
    {
		if(shouldUseAbility && Time.timeScale != 0)
		tpc.TwoAbility();
    }

	/// <summary>
	/// Calls for the ability in the Q position to be used.
	/// </summary>
	/// <param name="shouldUseAbility">Holds true if the ability should be used.</param>
	private void OneAbilityInput(bool shouldUseAbility)
	{
		if(shouldUseAbility && Time.timeScale != 0)
		tpc.OneAbility();
	}
	#endregion
	#endregion
	#endregion
}