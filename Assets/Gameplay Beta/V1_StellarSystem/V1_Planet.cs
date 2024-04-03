using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Planet : MonoBehaviour
{
	public PlanetData data;
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
public class PlanetData
{
	public float mass;
	public float radius;
	public float distance;

	public PlanetData()
	{
		mass = 0;
		radius = 0;
		distance = 0;
	}

	public PlanetData Randomize()
	{
		mass = 0.5f * UnityEngine.Random.value + 0.5f;
		radius = 0.5f * UnityEngine.Random.value + 0.1f;
		distance = 4f * UnityEngine.Random.value + 1f;
		return this;
	}
}