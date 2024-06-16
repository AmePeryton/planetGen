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
	public V1_Star star;
	public V1_MainPlanet mainPlanet;
	public List<V1_OtherPlanet> otherplanets;
	public float[] habitableZone;

	[Header("Prefabs")]
	public GameObject starPrefab;
	public GameObject mainPlanetPrefab;
	public GameObject otherPlanetPrefab;
	public GameObject moonPrefab;

	[Header("Visual Scales")]
	public float starScale;
	public float planetScale;
	public float moonScale;

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

		// Spawn star as defined in the save data
		GameObject newStarObj = Instantiate(starPrefab);
		V1_Star newStar = newStarObj.GetComponent<V1_Star>();
		newStar.data = saveData.starData;
		star = newStar;

		// Spawn main planet as defined in the save data
		GameObject newMainPlanetObj = Instantiate(mainPlanetPrefab);
		V1_MainPlanet newMainPlanet = newMainPlanetObj.GetComponent<V1_MainPlanet>();
		newMainPlanet.data = saveData.mainPlanetData;
		mainPlanet = newMainPlanet;
		foreach (MoonData moon in mainPlanet.data.moons)
		{
			GameObject newMoonObj = Instantiate(moonPrefab, mainPlanet.transform);
			V1_Moon newMoon = newMoonObj.GetComponent<V1_Moon>();
			newMoon.data = moon;
		}

		// Spawn other planets as listed in the save data
		foreach (OtherPlanetData otherPlanet in saveData.otherPlanetData)
		{
			GameObject newOtherPlanetObj = Instantiate(otherPlanetPrefab);
			V1_OtherPlanet newOtherPlanet = newOtherPlanetObj.GetComponent<V1_OtherPlanet>();
			newOtherPlanet.data = otherPlanet;
			otherplanets.Add(newOtherPlanet);
			foreach (MoonData moon in newOtherPlanet.data.moons)
			{
				GameObject newMoonObj = Instantiate(moonPrefab, newOtherPlanet.transform);
				V1_Moon newMoon = newMoonObj.GetComponent<V1_Moon>();
				newMoon.data = moon;
			}
		}
	}

	public override void SaveData()
	{
		V1_GameSaveDataManager.instance.genericSaveData.dateModified = DateTime.Now.ToString("yyyy-MM-dd");
		saveData = new V1_StellarSystemSaveData(V1_GameSaveDataManager.instance.genericSaveData);

		// Save star data from the star object
		saveData.starData = star.data;

		// Save main planet data from the main planet object
		saveData.mainPlanetData = mainPlanet.data;
		//foreach (MoonData moon in mainPlanet.data.moons)
		//{
		//	V1_Moon newMoon = newMoonObj.GetComponent<V1_Moon>();
		//	newMoon.data = moon;
		//}

		// Save other planet data from other planet objects
		foreach (V1_OtherPlanet otherPlanet in otherplanets)
		{
			saveData.otherPlanetData.Add(otherPlanet.data);
		}

		V1_FileHandler.Save(saveData, Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + saveData.name + ".save");
	}

	public void NewStellarSystem()
	{
		// New Star
		GameObject newStarObj = Instantiate(starPrefab);
		V1_Star newStar = newStarObj.GetComponent<V1_Star>();
		newStar.RandomizeProperties();
		star = newStar;

		// Calculate habitable zone
		habitableZone = new float[]
		{
			Mathf.Sqrt(star.data.luminosity/1.1f),
			Mathf.Sqrt(star.data.luminosity/0.53f)
		};

		// New Main Planet
		GameObject newMainPlanetObj = Instantiate(mainPlanetPrefab);
		V1_MainPlanet newMainPlanet = newMainPlanetObj.GetComponent<V1_MainPlanet>();
		newMainPlanet.RandomizeProperties();
		mainPlanet = newMainPlanet;
		int numMoons = UnityEngine.Random.Range(0, 3);
		for (int i = 0; i < numMoons; i++)
		{
			GameObject newMoonObj = Instantiate(moonPrefab, mainPlanet.transform);
			V1_Moon newMoon = newMoonObj.GetComponent<V1_Moon>();
			newMoon.RandomizeProperties();
			mainPlanet.data.moons.Add(newMoon.data);
		}

		// New Planets
		int numPlanets = UnityEngine.Random.Range(0, 4);
		for (int i = 0; i < numPlanets; i++)
		{
			GameObject newPlanetObj = Instantiate(otherPlanetPrefab);
			V1_OtherPlanet newPlanet = newPlanetObj.GetComponent<V1_OtherPlanet>();
			newPlanet.RandomizeProperties();
			otherplanets.Add(newPlanet);

			numMoons = UnityEngine.Random.Range(0, 3);
			for (int j = 0; j < numMoons; j++)
			{
				GameObject newMoonObj = Instantiate(moonPrefab, newPlanetObj.transform);
				V1_Moon newMoon = newMoonObj.GetComponent<V1_Moon>();
				newMoon.RandomizeProperties();
				newPlanet.data.moons.Add(newMoon.data);
			}
		}
	}
}

[Serializable]
public class V1_StellarSystemSaveData : V1_GameSaveData
{
	public StarData starData;
	public MainPlanetData mainPlanetData;
	public List<OtherPlanetData> otherPlanetData;

	public V1_StellarSystemSaveData(V1_GameSaveData generic) : base(generic.name)
	{
		//name = generic.name;
		//dateCreated = generic.dateCreated;
		dateModified = generic.dateModified;
		gameState = GameState.StellarSystem;

		starData = new StarData();
		mainPlanetData = new MainPlanetData();
		otherPlanetData = new List<OtherPlanetData>();
	}
}