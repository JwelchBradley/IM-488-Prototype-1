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

	/// <summary>
	/// Holds true if the player wants to shoot.
	/// </summary>
	private bool shoot;

	/// <summary>
	/// Holds true if the player wants to shoot.
	/// </summary>
	public bool Shoot
    {
		get => shoot;
    }
    #endregion
    #endregion

    [Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

	/// <summary>
	/// The pause menu in this scene.
	/// </summary>
	private PauseMenuBehavior pmb;
	#endregion

	#region Functions
	#region Initialization
	private void Awake()
    {
		pmb = GameObject.Find("Pause Menu Templates Canvas").GetComponent<PauseMenuBehavior>();
    }
    #endregion

    #region Input recievers
    #region Movement
    public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}

	public void OnMoveFast(InputValue value)
	{
		SprintInput(value.isPressed);
	}

	public void OnDash(InputValue value)
    {
		DashInput(value.Get<Vector2>());
    }

	public void OnMoveVertically(InputValue value)
	{
		VerticalInput(value.Get<float>());
	}

	public void OnMoveDown(InputValue value)
    {

    }
	#endregion

	public void OnLook(InputValue value)
	{
		if (cursorInputForLook)
		{
			LookInput(value.Get<Vector2>());
		}
	}

	public void OnShoot(InputValue value)
    {
		ShootInput(value.isPressed);
    }

	public void OnPause()
    {
		pmb.PauseGame();
    }
    #endregion

    #region Input Updaters
    public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	}

	private void DashInput(Vector2 newDashDirection)
    {
		Debug.Log(newDashDirection);
    }

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void VerticalInput(float newVerticalState)
	{
		moveVertical = newVerticalState;
	}

	public void SprintInput(bool newMoveFastState)
	{
		moveFast = newMoveFastState;
	}

	private void ShootInput(bool newShootState)
    {
		shoot = newShootState;
    }
    #endregion	
    #endregion
}