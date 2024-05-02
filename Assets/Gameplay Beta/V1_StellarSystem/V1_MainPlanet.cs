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
public class MainPlanetData : OtherPlanetData
{
	public string name;

	public MainPlanetData()
	{
		mass = 0;
		radius = 0;
		distance = 0;

		name = "initial main planet";
	}

	public new MainPlanetData Randomize()
	{
		mass = 0.4f * UnityEngine.Random.value + 0.8f;
		radius = 0.2f * UnityEngine.Random.value + 0.9f;
		distance = 0.2f * UnityEngine.Random.value + 0.9f;

		name = "randomized main planet";
		return this;
	}

	public new MainPlanetData Default()
	{
		mass = 1;
		radius = 1;
		distance = 1;
		return this;
	}
}