using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_StellarCamera : MonoBehaviour
{

	void Start()
	{
		transform.position = new Vector3(0, 10, 0);
		transform.LookAt(Vector3.zero);
	}

	void Update()
	{
		
	}
}