using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_OtherPlanet : MonoBehaviour
{
	public OtherPlanetData data;
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
public class OtherPlanetData
{
	public float mass;		// in Earth Masses
	public float radius;	// in Earth Radii
	public float distance;  // in AU
	public List<MoonData> moons;

	public OtherPlanetData()
	{
		mass = 0;
		radius = 0;
		distance = 0;
		moons = new List<MoonData>();
	}

	public OtherPlanetData Randomize()
	{
		mass = 0.8f * UnityEngine.Random.value + 0.2f;
		radius = 0.5f * UnityEngine.Random.value + 0.1f;
		distance = 4f * UnityEngine.Random.value + 1f;
		//moons = new List<V1_Moon>();
		return this;
	}

	// Earth's current properties
	public OtherPlanetData Default()
	{
		mass = 1;
		radius = 1;
		distance = 1;
		return this;
		//moons = new List<V1_Moon>();
	}
}