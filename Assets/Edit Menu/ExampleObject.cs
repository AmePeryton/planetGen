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

	[MenuEditable("Int Test", -10, 10)]
	public int intTest;

	[MenuEditable("String Test")]
	public string stringTest;

	[MenuEditable("Vector2 Test")]
	public Vector2 Vector2Test;

	[MenuEditable("Vector3 Test")]
	public Vector3 Vector3Test;

	// This variable will show in the stat menu, but will not be directly editable from it
	[MenuEditable("Read Only", false)]
	public Vector3 ReadOnlyTest;

	// This variable show up as a warning in the stat menu, since arrays are not (yet) implemented
	[MenuEditable("Error Test")]
	public int[] errorTest;

	// This variable will not show in the stat menu, but will not send a warning message, since it is private
	[MenuEditable("Private Test")]
	private string privateTest;

	private Material material;

	private void Awake()
	{
		material = GetComponent<Renderer>().material;
		RandomizeVariables();
	}

	void Update()
	{
		ReadOnlyTest = Vector3Test * intTest;
		transform.position = ReadOnlyTest;
		transform.localScale = floatTest * new Vector3(Vector2Test.x, Vector2Test.y, 1);
		material.color = colorTest;
	}

	private void RandomizeVariables()
	{
		floatTest = UnityEngine.Random.Range(0.5f, 4f);
		intTest = UnityEngine.Random.Range(0, 4);
		Vector2Test = new Vector2(UnityEngine.Random.Range(0.5f, 4f), UnityEngine.Random.Range(0.5f, 4f));
		Vector3Test = UnityEngine.Random.insideUnitSphere;
	}

	public string GetName()
	{
		return "Example Object " + objectName;
	}

	public string GetInfo()
	{
		return "This is a " + typeof(ExampleObject).Name + ". The string is " + stringTest;
	}
}