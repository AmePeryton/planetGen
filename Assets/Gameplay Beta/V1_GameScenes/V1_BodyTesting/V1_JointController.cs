using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_JointController : MonoBehaviour
{
	public V1_JointData data;

	private void Awake()
	{

	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}
}

public enum JointType
{
	Fixed,
	Hinge,
	Pivot,
	Sadle,
	Ball
}

[Serializable]
public class V1_JointData
{
	public JointType jointType;
	public Vector3 position;
}