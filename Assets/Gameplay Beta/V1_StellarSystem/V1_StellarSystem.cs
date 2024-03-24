using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class V1_StellarSystem : MonoBehaviour, IStellarSystemSavable
{
	public static V1_StellarSystem instance { get; private set; }
	public GameObject starPrefab;
	public GameObject planetPrefab;
	public List<V1_Star> stars;
	public List<V1_Planet> planets;

	private void Awake()
	{
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	public void NewStellarSystem()
	{
		// New Star
		{
			GameObject newObj = Instantiate(starPrefab);
			V1_Star newStar = newObj.GetComponent<V1_Star>();
			newStar.RandomizeProperties();
			stars.Add(newStar);
		}

		// New Planets
		int numPlanets = Random.Range(1, 4);
		for (int i = 0; i < numPlanets; i++)
		{
			GameObject newObj = Instantiate(planetPrefab);
			V1_Planet newPlanet = newObj.GetComponent<V1_Planet>();
			newPlanet.RandomizeProperties();
			planets.Add(newPlanet);
		}
	}

	public void LoadData(V1_StellarSystemSaveData data)
	{
		// Spawn star as listed in the save data
		foreach (StarData star in data.starData)
		{
			GameObject newObj = Instantiate(starPrefab);
			V1_Star newStar = newObj.GetComponent<V1_Star>();
			newStar.mass = star.mass;
			newStar.age = star.age;
			stars.Add(newStar);
		}

		// Spawn planets as listed in the save data
		foreach (PlanetData planet in data.planetData)
		{
			GameObject newObj = Instantiate(planetPrefab);
			V1_Planet newPlanet = newObj.GetComponent<V1_Planet>();
			newPlanet.mass = planet.mass;
			newPlanet.radius = planet.radius;
			newPlanet.distance = planet.distance;
			planets.Add(newPlanet);
		}
	}

	public void SaveData(ref V1_StellarSystemSaveData data)
	{
		// Save star data from each star object
		foreach (V1_Star star in stars)
		{
			StarData newStarData = new StarData
			{
				mass = star.mass,
				age = star.age
			};
			data.starData.Add(newStarData);
		}

		// Save planet data from planet star object
		foreach (V1_Planet planet in planets)
		{
			PlanetData newPlanetData = new PlanetData
			{
				mass = planet.mass,
				radius = planet.radius,
				distance = planet.distance
			};
			data.planetData.Add(newPlanetData);
		}
	}
}