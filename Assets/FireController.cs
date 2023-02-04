using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
	// 2/4/8/12/24 wisps at 12/6/3/2/1 second spawn
	[SerializeField]
	private GameObject firePrefab = null;

	[SerializeField]
	private int numFires = 100;

	[SerializeField]
	private int initialOrbitCount = 0;

	[SerializeField]
	private int maxOrbiting = 8;

	[SerializeField]
	private float fullOrbitRegen = 24.0f;

	[SerializeField]
	private float orbitOffset = 1.5f;

	[SerializeField]
	private float orbitOffsetVariance = 0.5f;

	[SerializeField]
	private Vector2 centerOffset = Vector2.one;

	private Queue<TrackTarget> disabledFire = new Queue<TrackTarget>();
	private List<TrackTarget> orbitingFire = new List<TrackTarget>();
	private List<TrackTarget> detachedFire = new List<TrackTarget>();

	public void ThrowFlame(Vector2 target)
	{
		if (orbitingFire.Count > 0)
		{
			int fireIndex = Random.Range(0, orbitingFire.Count);
			TrackTarget fire = orbitingFire[fireIndex];
			orbitingFire.RemoveAt(fireIndex);
			detachedFire.Add(fire);
			fire.SetTarget(target);
		}
	}

	private void Start()
	{
		for(int i = 0; i < numFires; i++)
		{
			GameObject fireObject = Instantiate(firePrefab, null, true);
			fireObject.SetActive(false);

			disabledFire.Enqueue(fireObject.GetComponent<TrackTarget>());
		}

		for(int i = 0; i < initialOrbitCount; i++)
		{
			SpawnFlame();
		}

		StartCoroutine(FlameSpawnTimer());
	}

	private void Update()
	{
		if (orbitingFire.Count > 0)
		{
			Vector2 position = transform.position;
			int orbitCount = orbitingFire.Count;
			float orbitAngleIncrement = Mathf.PI * 2.0f / maxOrbiting;
			float facingRotation = transform.eulerAngles.z * Mathf.Deg2Rad;

			for (int i = 0; i < orbitCount; i++)
			{
				float angle = orbitAngleIncrement * i;
				float localOffset = Mathf.Sin(angle) * orbitOffsetVariance + orbitOffset;
				angle += facingRotation;
				orbitingFire[i].SetTarget(
					position.x + Mathf.Cos(angle) * localOffset - transform.up.x * centerOffset.x,
					position.y + Mathf.Sin(angle) * localOffset - transform.up.y * centerOffset.y);
			}
		}
	}

	private void SpawnFlame()
	{
		if (disabledFire.Count > 0)
		{
			TrackTarget fire = disabledFire.Dequeue();
			fire.transform.position = transform.position - Vector3.forward;
			orbitingFire.Add(fire);
			fire.gameObject.SetActive(true);
		}
	}

	private IEnumerator FlameSpawnTimer()
	{
		while (true)
		{
			yield return new WaitForSeconds(fullOrbitRegen / maxOrbiting);

			if (orbitingFire.Count < maxOrbiting)
			{
				SpawnFlame();
			}
		}
	}
}
