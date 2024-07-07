using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/OrganType")]
public class V1_OrganType : ScriptableObject
{
	public string organName;
	public IntakeValues intakes;
	public OutputValues outputs;

	public Color color;
}

[Serializable]
public struct IntakeValues
{
	public float energy;
	public float calories;
	public float minerals;
	public float light;
	public float oxygen;
	public float co2;
	public float neuron;
}

[Serializable]
public struct OutputValues
{
	public float energy;
	public float calories;
	public float minerals;
	public float light;
	public float oxygen;
	public float co2;
	public float neuron;
}