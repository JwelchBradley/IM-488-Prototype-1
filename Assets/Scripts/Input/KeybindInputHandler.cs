/*****************************************************************************
// File Name :         KeybindInputHandler.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles user inputs.
*****************************************************************************/
using System;
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
	/// <summary>
	/// Gets the input move values.
	/// </summary>
	/// <param name="value">Input move value.</param>
    public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}

	/// <summary>
	/// Gets the input move fast values.
	/// </summary>
	/// <param name="value">Input move fast value.</param>
	public void OnMoveFast(InputValue value)
	{
		MoveFastInput(value.isPressed);
	}

    #region Dash
    /// <summary>
    /// Gets the input dash values.
    /// </summary>
    /// <param name="value">Input dash value.</param>
    public void OnDashRight(InputValue value)
    {
		DashInput(new Vector3(1, 0, 0));
    }

	/// <summary>
	/// Gets the input dash values.
	/// </summary>
	/// <param name="value">Input dash value.</param>
	public void OnDashLeft(InputValue value)
	{
		DashInput(new Vector3(-1, 0, 0));
	}

	/// <summary>
	/// Gets the input dash values.
	/// </summary>
	/// <param name="value">Input dash value.</param>
	public void OnDashForward(InputValue value)
	{
		DashInput(new Vector3(0, 0, 1));
	}

	/// <summary>
	/// Gets the input dash values.
	/// </summary>
	/// <param name="value">Input dash value.</param>
	public void OnDashBack(InputValue value)
	{
		DashInput(new Vector3(0, 0, -1));
	}

	/// <summary>
	/// Gets the input dash values.
	/// </summary>
	/// <param name="value">Input dash value.</param>
	public void OnDashUp(InputValue value)
	{
		DashInput(new Vector3(0, 1, 0));
	}

	/// <summary>
	/// Gets the input dash values.
	/// </summary>
	/// <param name="value">Input dash value.</param>
	public void OnDashDown(InputValue value)
	{
		DashInput(new Vector3(0, -1, 0));
	}
	#endregion

	/// <summary>
	/// Gets the input move vertically values.
	/// </summary>
	/// <param name="value">Input move vertically value.</param>
	public void OnMoveVertically(InputValue value)
	{
		VerticalInput(value.Get<float>());
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
    public void OnXAbility(InputValue value)
    {
		XAbilityInput(value.isPressed);
    }

	/// <summary>
	/// Gets the input E ability values.
	/// </summary>
	/// <param name="value">Input E ability value.</param>
	public void OnEAbility(InputValue value)
    {
		EAbilityInput(value.isPressed);
	}

	/// <summary>
	/// Gets the input Q ability values.
	/// </summary>
	/// <param name="value">Input Q ability value.</param>
	public void OnQAbility(InputValue value)
	{
		QAbilityInput(value.isPressed);
	}

	public void OnADS(InputValue value)
    {
		OnADSInput(value.isPressed);
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
	/// Gets the mouse delta movement since the last frame.
	/// </summary>
	/// <param name="newLookDirection">The delta mouse movement.</param>
	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
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
	#endregion

	#region Shoot
	/// <summary>
	/// Calls for the player to shoot.
	/// </summary>
	/// <param name="shouldShoot">Holds true if the player should shoot.</param>
	private void ShootInput(bool shouldShoot)
	{
		tpc.Shoot(shouldShoot);
	}

	private void OnADSInput(bool shouldADS)
    {
		tpc.ADS(shouldADS);
    }
	#endregion

	#region Abilities
	/// <summary>
	/// Calls for the ability in the alt click position to be used.
	/// </summary>
	/// <param name="shouldUseAbility">Holds true if the ability should be used.</param>
	private void XAbilityInput(bool shouldUseAbility)
    {
		if(shouldUseAbility)
		tpc.XAbility();
    }

	/// <summary>
	/// Calls for the ability in the E position to be used.
	/// </summary>
	/// <param name="shouldUseAbility">Holds true if the ability should be used.</param>
	private void EAbilityInput(bool shouldUseAbility)
    {
		if(shouldUseAbility)
		tpc.EAbility();
    }

	/// <summary>
	/// Calls for the ability in the Q position to be used.
	/// </summary>
	/// <param name="shouldUseAbility">Holds true if the ability should be used.</param>
	private void QAbilityInput(bool shouldUseAbility)
	{
		if(shouldUseAbility)
		tpc.QAbility();
	}
	#endregion
	#endregion
	#endregion
}