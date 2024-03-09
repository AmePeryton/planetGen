using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_DistributorScript : MonoBehaviour
{
	public int numObjects;

	public float min;
	public float max;
	public float exponent;
	public RandomFromDistribution.Direction_e direction;

	void Start()
	{
		for (int i = 0; i < numObjects; i++)
		{
			Vector3 randPos = new Vector3(RandomFromDistribution.RandomRangeExponential(min, max, exponent, direction), 0,0);
			GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = randPos;
		}
	}

	void Update()
	{
		
	}
}