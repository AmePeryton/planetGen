using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_StarController : MonoBehaviour
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
		//sphere.transform.localScale = Mathf.Pow(data.mass, 0.74f) * Vector3.one;
		sphere.transform.localScale = data.radius * Vector3.one * 
			V1_StellarSystemController.instance.gsd.starScale * 
			V1_StellarUnits.solarRadius_km / V1_StellarUnits.AU_km;
		sphere.GetComponent<Renderer>().material.color = data.color;
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
	public float age;			// in billions of years (Gyr)
	public float mass;			// in Solar Masses
	public float temperature;	// in Kelvins, surface
	public float radius;		// in Solar Radii
	public float luminosity;	// in Solar Luminosity (since Watts would be too large)
	public Color color;			// Base color
	//public List<StarLayer> layers;

	public StarData()
	{
		age = 0;
		mass = 0;
		temperature = 0;
		radius = 0;
		luminosity = 0;
		color = Color.black;
	}

	public StarData Randomize()
	{
		age = 6f * UnityEngine.Random.value;
		mass = UnityEngine.Random.value + 0.5f;
		temperature = 5780;		// PLACEHOLDER
		radius = 4f * UnityEngine.Random.value;
		luminosity = 1;			// PLACEHOLDER
		color = Color.yellow;	// PLACEHOLDER
		/*layers.Add(new StarLayer("Core")
		{
			innerRadius = 0,
			outerRadius = 0,
			temperature = 15000000,
			density = 150,
			pressure = 3840000000000,
			mass = 1,
			energyOutput = 0,
			elements = new float[]
			{
				0.74f,
				0.24f,
				0.02f
			}
		});*/
		
		return this;
	}

	// Our sun's current properties
	public StarData Default()
	{
		age = 4.603f;
		mass = 1;
		temperature = 5772;
		radius = 1;
		luminosity = 1;
		color = Color.yellow;
		return this;
	}
}

[Serializable]
public class StarLayer
{
	public string name;
	public float innerRadius;
	public float outerRadius;
	public float temperature;
	public float density;
	public float pressure;
	public float mass;
	public float energyOutput;
	public float[] elements;

	public StarLayer(string name)
	{
		this.name = name;
		innerRadius = 0;
		outerRadius = 0;
		temperature = 0;
		density = 0;
		pressure = 0;
		mass = 0;
		energyOutput = 0;
		elements = new float[]
		{

		};
	}
}