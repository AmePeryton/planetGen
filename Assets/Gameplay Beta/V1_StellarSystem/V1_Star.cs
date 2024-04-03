using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Star : MonoBehaviour
{
	public StarData data;
	public GameObject sphere;

	private void Awake()
	{
	}

	void Start()
	{
	}

	void Update()
	{
		VisualUpdate();
		// NOTE: OK for early development, but later the constant refreshing will cause low fps
	}

	private void VisualUpdate()
	{
		sphere.transform.localScale = Mathf.Pow(data.mass, 0.74f) * Vector3.one;
	}

	public void RandomizeProperties()
	{
		data.Randomize();
		VisualUpdate();
	}
}

[Serializable]
public class StarData
{
	public float mass;
	public float age;

	public StarData()
	{
		mass = 0;
		age = 0;
	}

	public StarData Randomize()
	{
		mass = UnityEngine.Random.value + 0.5f;
		age = 4f * UnityEngine.Random.value;
		return this;
	}
}