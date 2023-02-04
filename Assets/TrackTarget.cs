using UnityEngine;

public class TrackTarget : MonoBehaviour
{
	[SerializeField]
	public float trackSpeed = 1.0f;

	private Vector3 target = Vector3.zero;

	private void Start()
	{
		target.z = transform.position.z;
	}

	public void SetTarget(Vector2 newTarget)
	{
		target.x = newTarget.x;
		target.y = newTarget.y;
	}

	public void SetTarget(float x, float y)
	{
		target.x = x;
		target.y = y;
	}

	private void FixedUpdate()
	{
		transform.Translate((target - transform.position) * trackSpeed);
	}
}
