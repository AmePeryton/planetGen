using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_StellarCamera : MonoBehaviour
{
	private void Awake()
	{
		transform.position = new Vector3(0, 10, 0);
		transform.LookAt(Vector3.zero);
	}

	void Start()
	{

	}

	void Update()
	{
		
	}
}