using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Moon : MonoBehaviour
{
	public MoonData data;
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
		transform.localPosition = new Vector3(0, 0, data.distance);
		sphere.transform.localScale = data.radius * Vector3.one;
	}

	public void RandomizeProperties()
	{
		data.Randomize();
		VisualUpdate();
	}
}

[Serializable]
public class MoonData
{
	public float mass;      // in Earth Masses
	public float radius;    // in Earth Radii
	public float distance;  // in AU

	public MoonData()
	{
		mass = 0;
		radius = 0;
		distance = 0;
	}

	public MoonData Randomize()
	{
		mass = 0.1f * UnityEngine.Random.value + 0f;
		radius = 0.2f * UnityEngine.Random.value + 0.1f;
		distance = 0.1f * UnityEngine.Random.value + 0.01f;
		return this;
	}

	// Earth's Moon's current properties
	public MoonData Default()
	{
		mass = 0.0123032f;
		radius = 0.2728069f;
		distance = 0.00256955529f;
		return this;
	}
}