using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Orbit : MonoBehaviour
{
	public float semimajorAxisLength;
	public float eccentricity;
	public float inclination;
	public float longitudeAscNode;
	public float argumentOfPeriapsis;

	public float semiminorAxisLength;
	public Vector3 trueCenter;

	void Start()
	{
		
	}

	void Update()
	{
		// sqrt(a^2 * (1 - e^2))
		semiminorAxisLength = Mathf.Sqrt(Mathf.Pow(semimajorAxisLength, 2) * (1 - Mathf.Pow(eccentricity, 2)));
		trueCenter = new Vector3(-semimajorAxisLength * eccentricity, 0, 0);

		transform.position = trueCenter;
		transform.localScale = new Vector3(semimajorAxisLength, 1, semiminorAxisLength);
		transform.rotation = Quaternion.identity;
		transform.RotateAround(Vector3.zero, Vector3.forward, inclination);
		transform.RotateAround(Vector3.zero, transform.up, argumentOfPeriapsis);
		transform.RotateAround(Vector3.zero, Vector3.up, longitudeAscNode);
	}
}