using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// New combined manager that handles save/loads AND the objects in the scene
public class V1_StellarSystemController : V1_SceneController
{
	public static new V1_StellarSystemController instance { get; private set; }
	public V1_StellarSystemSaveData saveData;
	public GameObject starPrefab;
	public GameObject planetPrefab;
	public List<V1_Star> stars;
	public List<V1_Planet> planets;

	private void Awake()
	{
		SceneControllerInit();
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	void Start()
	{
		// Return to menu if game is opened in stellar system scene
		if (V1_GameSaveDataManager.instance == null)
		{
			Debug.LogWarning("V1_GameSaveDataManager not found! Returning to menu");
			SceneManager.LoadScene("V1_SCENE_MainMenu", LoadSceneMode.Single);
			return;
		}

		if (V1_GameSaveDataManager.instance.newGame)
		{
			NewGame();
		}
		else
		{
			LoadData();
		}
	}

	// Create new data without loading existing data
	public void NewGame()
	{
		saveData = new V1_StellarSystemSaveData(V1_GameSaveDataManager.instance.genericSaveData);
		NewStellarSystem();
	}

	public override void LoadData()
	{
		saveData = V1_FileHandler.Load<V1_StellarSystemSaveData>
			(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + V1_GameSaveDataManager.instance.genericSaveData.name + ".save");

		// Spawn star as listed in the save data
		foreach (StarData star in saveData.starData)
		{
			GameObject newObj = Instantiate(starPrefab);
			V1_Star newStar = newObj.GetComponent<V1_Star>();
			newStar.data = star;
			stars.Add(newStar);
		}

		// Spawn planets as listed in the save data
		foreach (PlanetData planet in saveData.planetData)
		{
			GameObject newObj = Instantiate(planetPrefab);
			V1_Planet newPlanet = newObj.GetComponent<V1_Planet>();
			newPlanet.data = planet;
			planets.Add(newPlanet);
		}
	}

	public override void SaveData()
	{
		V1_GameSaveDataManager.instance.genericSaveData.dateModified = DateTime.Now.ToString("yyyy-MM-dd");
		saveData = new V1_StellarSystemSaveData(V1_GameSaveDataManager.instance.genericSaveData);

		// Save star data from each star object
		foreach (V1_Star star in stars)
		{
			saveData.starData.Add(star.data);
		}

		// Save planet data from planet star object
		foreach (V1_Planet planet in planets)
		{
			saveData.planetData.Add(planet.data);
		}

		V1_FileHandler.Save(saveData, Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + saveData.name + ".save");
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
		int numPlanets = UnityEngine.Random.Range(1, 4);
		for (int i = 0; i < numPlanets; i++)
		{
			GameObject newObj = Instantiate(planetPrefab);
			V1_Planet newPlanet = newObj.GetComponent<V1_Planet>();
			newPlanet.RandomizeProperties();
			planets.Add(newPlanet);
		}
	}
}