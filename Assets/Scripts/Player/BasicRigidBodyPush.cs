/*****************************************************************************
// File Name :         BasicRigibodyPush.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Allows this character controller to push rigidbodies.
*****************************************************************************/
using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
	[Tooltip("The layers that this object can push")]
	[SerializeField]
	private LayerMask pushLayers;

	[Tooltip("Holds true if this object can currently push other objects")]
	[SerializeField]
	private bool canPush;

	[Tooltip("The force that this character can push objects")]
	[Range(0.5f, 5f)] 
	[SerializeField]
	private float strength = 1.1f;

	/// <summary>
	/// If the character collides with an object, push it.
	/// </summary>
	/// <param name="hit"></param>
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (canPush) PushRigidBodies(hit);
	}

	/// <summary>
	/// Pushes rigidbodies.
	/// </summary>
	/// <param name="hit"></param>
	private void PushRigidBodies(ControllerColliderHit hit)
	{
		// https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html

		// make sure we hit a non kinematic rigidbody
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) return;

		// make sure we only push desired layer(s)
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0) return;

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3f) return;

		// Calculate push direction from move direction, horizontal motion only
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

		// Apply the push and take strength into account
		body.AddForce(pushDir * strength, ForceMode.Impulse);
	}
}