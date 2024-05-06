using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_MainPlanet : MonoBehaviour
{
	public MainPlanetData data;
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
public class MainPlanetData
{
	public string name;
	public float mass;      // in Earth Masses
	public float radius;    // in Earth Radii
	public float distance;  // in AU
	public List<MoonData> moons;

	public MainPlanetData()
	{
		name = "initial main planet";
		mass = 0;
		radius = 0;
		distance = 0;
		moons = new List<MoonData>();
	}

	public MainPlanetData Randomize()
	{
		name = "randomized main planet";
		mass = 0.4f * UnityEngine.Random.value + 0.8f;
		radius = 0.2f * UnityEngine.Random.value + 0.9f;
		distance = 0.2f * UnityEngine.Random.value + 0.9f;
		return this;
	}

	public MainPlanetData Default()
	{
		mass = 1;
		radius = 1;
		distance = 1;
		return this;
	}
}