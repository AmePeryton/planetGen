using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TissueOrb : MonoBehaviour
{
	public Tissue tissue;   // the tissue instance that this orb represents

	private Material material;

	private void Awake()
	{
		material = GetComponent<Renderer>().material;
	}

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
		material.color = tissue.tissueType.color;
	}
}