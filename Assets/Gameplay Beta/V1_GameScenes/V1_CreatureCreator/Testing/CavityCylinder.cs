using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavityCylinder : MonoBehaviour
{
	public Cavity cavity;

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