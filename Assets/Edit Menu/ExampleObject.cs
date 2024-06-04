using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ExampleObject : MonoBehaviour, IMenuEditable
{
	public string objectName;

	[MenuEditable("Bool Test")]
	public bool boolTest;

	[MenuEditable("Color Test")]
	public Color colorTest;

	[MenuEditable("Float Test", 0.1f, 10)]
	public float floatTest;

	[MenuEditable("Int Test", 0, 20)]
	public int intTest;

	[MenuEditable("String Test")]
	public string stringTest;

	[MenuEditable("Vector2 Test", false)]
	public Vector2 Vector2Test;

	[MenuEditable("Vector3 Test")]
	public Vector3 Vector3Test;

	[MenuEditable("Error Test")]
	public int[] errorTest;

	public Material material;

	private void Awake()
	{
		floatTest = UnityEngine.Random.Range(0.1f, 2f);
		Vector3Test = UnityEngine.Random.insideUnitSphere;
		material = GetComponent<Renderer>().material;
		//LeanTween.value(gameObject, 0.1f, 4, 5).setEaseInOutSine().setLoopPingPong().setOnUpdate((float value) => { 
		//	floatTest = value;
		//});
	}

	void Update()
	{
		transform.position = Vector3Test * intTest;
		transform.localScale = floatTest * Vector3.one; 
		material.color = colorTest;

		Vector2Test = Vector3Test / 2;
		//floatTest += 0.001f;
	}

	public string GetName()
	{
		return "Example Object " + objectName;
	}

	public string GetInfo()
	{
		return "This is a " + typeof(ExampleObject).Name;
	}
}