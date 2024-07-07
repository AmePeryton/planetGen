using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_AxialDrag : MonoBehaviour
{
	public float dragConstant = 1.17f * 1.225f / 2f;   // represents (coefficient of drag for rectangular prism * fluid density / 2)
	public float dirVel = 0f;
	public bool directional;
	public float[] dragMults = { 1f, 1f, 1f, 1f, 1f, 1f }; // directional drag multipliers in order: -x, +x, -y, +y, -z, +z
	public float[] realDrag = new float[6];
	public Vector3 dragMult = new Vector3(1f, 1f, 1f);
	public Vector3 area = new Vector3(1f, 1f, 1f); // cross sectional areas
	public Rigidbody rigidBody;
	void Start()
	{
		
	}

	void Update()
	{
		
	}
}