using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCube : MonoBehaviour
{
	public Environment environment;

	void Start()
	{
		transform.position = Random.onUnitSphere;
		UpdateDisplay();
	}

	void Update()
	{
		//UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		transform.localScale = Vector3.one;
	}
}