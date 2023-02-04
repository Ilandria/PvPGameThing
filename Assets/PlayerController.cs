using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Camera playerCamera = null;

	[SerializeField]
	private GameObject playerObject = null;

	[SerializeField]
	private Rigidbody2D playerRigidbody = null;

	[SerializeField]
	private float movementForce = 50.0f;

	[SerializeField]
	private float turnForce = 50.0f;

	[SerializeField]
	private UnityEvent<Vector2> onBasic = null;

	private Vector2 rawMovementInput = Vector2.zero;
	private Vector3 rawLookInput = Vector2.zero;

	private void FixedUpdate()
	{
		playerRigidbody.AddForce(rawMovementInput * movementForce);

		Vector3 lookDirection = rawLookInput - playerObject.transform.position;
		lookDirection.z = 0.0f;
		lookDirection.Normalize();
		playerRigidbody.AddTorque(Vector3.Cross(playerObject.transform.up, lookDirection).z * turnForce);
	}

	public void OnMove(InputValue value)
	{
		rawMovementInput = value.Get<Vector2>();
	}

	public void OnLook(InputValue value)
	{
		rawLookInput = playerCamera.ScreenToWorldPoint(value.Get<Vector2>());
	}

	public void OnBasic(InputValue value)
	{
		onBasic?.Invoke(rawLookInput);
	}
}
