using System;
using System.Collections.Generic;
using UnityEngine;

// New combined manager that handles save/loads AND the objects in the scene
public class V1_StellarSystemController : V1_SceneController
{
	public static new V1_StellarSystemController instance { get; private set; }

	[Header("Savable Data")]
	public V1_SaveData_StellarSystem stellarSystemData;
	public V1_GameSceneData_StellarSystem gsd;

	[Header("Controllers")]
	public V1_StarController starController;
	public V1_MainPlanetController mainPlanetController;
	public List<V1_OtherPlanetController> otherplanetControllers;

	[Header("Prefabs")]
	public GameObject starPrefab;
	public GameObject mainPlanetPrefab;
	public GameObject otherPlanetPrefab;
	public GameObject moonPrefab;

	private void Awake()
	{
		SceneControllerInit();
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	void Start()
	{
		if (V1_SaveDataManager.instance.data.common.gameScene == GameScene.StellarSystem)
		{
			// If save data is from this scene, load into game objects
			LoadScene();
		}
		else
		{
			// If save data is from a different scene, create new game scene data and new game
			NewScene();
		}
	}

	public override void LoadScene()
	{
		stellarSystemData = V1_SaveDataManager.instance.data.stellarSystem;
		gsd = V1_SaveDataManager.instance.data.gsd_stellarSystem;

		// Spawn star as defined in the save data
		GameObject newStarObj = Instantiate(starPrefab);
		starController = newStarObj.GetComponent<V1_StarController>();
		starController.data = stellarSystemData.starData;

		// Spawn main planet as defined in the save data
		GameObject newMainPlanetObj = Instantiate(mainPlanetPrefab);
		mainPlanetController = newMainPlanetObj.GetComponent<V1_MainPlanetController>();
		mainPlanetController.data = stellarSystemData.mainPlanetData;
		foreach (MoonData moon in stellarSystemData.mainPlanetData.moons)
		{
			GameObject newMoonObj = Instantiate(moonPrefab, mainPlanetController.transform);
			V1_MoonController newMoon = newMoonObj.GetComponent<V1_MoonController>();
			newMoon.data = moon;
		}

		// Spawn other planets as listed in the save data
		foreach (OtherPlanetData otherPlanet in stellarSystemData.otherPlanetsData)
		{
			GameObject newOtherPlanetObj = Instantiate(otherPlanetPrefab);
			V1_OtherPlanetController newOtherPlanet = newOtherPlanetObj.GetComponent<V1_OtherPlanetController>();
			newOtherPlanet.data = otherPlanet;
			otherplanetControllers.Add(newOtherPlanet);
			foreach (MoonData moon in otherPlanet.moons)
			{
				GameObject newMoonObj = Instantiate(moonPrefab, newOtherPlanet.transform);
				V1_MoonController newMoon = newMoonObj.GetComponent<V1_MoonController>();
				newMoon.data = moon;
			}
		}
	}

	public override void NewScene()
	{
		V1_SaveDataManager.instance.data.common.gameScene = GameScene.StellarSystem;
		V1_SaveDataManager.instance.data.stellarSystem = stellarSystemData;
		V1_SaveDataManager.instance.data.gsd_stellarSystem = gsd;
		NewStellarSystem();
	}

	public override void SaveScene()
	{
		V1_SaveDataManager.instance.SaveData();
	}

	[ContextMenu("New Stellar System")]
	public void NewStellarSystem()
	{
		// Destroy old objects and clear data where necessary
		if (starController != null)
		{
			Destroy(starController.gameObject);
		}
		if (mainPlanetController != null)
		{
			Destroy(mainPlanetController.gameObject);
		}
		foreach (V1_OtherPlanetController otherPlanet in otherplanetControllers)
		{
			Destroy(otherPlanet.gameObject);
		}
		otherplanetControllers.Clear();
		stellarSystemData.otherPlanetsData.Clear();

		// New Star
		starController = Instantiate(starPrefab).GetComponent<V1_StarController>();
		starController.RandomizeProperties();
		stellarSystemData.starData = starController.data;

		// New Main Planet
		mainPlanetController = Instantiate(mainPlanetPrefab).GetComponent<V1_MainPlanetController>();
		mainPlanetController.RandomizeProperties();
		stellarSystemData.mainPlanetData = mainPlanetController.data;

		int numMoons = UnityEngine.Random.Range(0, 3);
		for (int i = 0; i < numMoons; i++)
		{
			V1_MoonController newMoon = Instantiate(moonPrefab, mainPlanetController.transform).GetComponent<V1_MoonController>();
			newMoon.RandomizeProperties();
			mainPlanetController.data.moons.Add(newMoon.data);
		}

		// New Planets
		int numPlanets = UnityEngine.Random.Range(1, 4);
		for (int i = 0; i < numPlanets; i++)
		{
			GameObject newOtherPlanetObj = Instantiate(otherPlanetPrefab);
			V1_OtherPlanetController newOtherPlanetController = newOtherPlanetObj.GetComponent<V1_OtherPlanetController>();
			otherplanetControllers.Add(newOtherPlanetController);
			newOtherPlanetController.RandomizeProperties();
			stellarSystemData.otherPlanetsData.Add(newOtherPlanetController.data);

			numMoons = UnityEngine.Random.Range(0, 3);
			for (int j = 0; j < numMoons; j++)
			{
				V1_MoonController newMoon = Instantiate(moonPrefab, newOtherPlanetObj.transform).GetComponent<V1_MoonController>();
				newMoon.RandomizeProperties();
				newOtherPlanetController.data.moons.Add(newMoon.data);
			}
		}
	}
}

[Serializable]
public class V1_GameSceneData_StellarSystem : V1_GameSceneData
{
	[Header("Visual Scales")]
	public float starScale;
	public float planetScale;
	public float moonScale;

	public V1_GameSceneData_StellarSystem()
	{
		starScale = 1.0f;
		planetScale = 1.0f;
		moonScale = 1.0f;
	}
}