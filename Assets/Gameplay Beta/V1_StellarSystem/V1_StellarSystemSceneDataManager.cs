using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages all gameobjects in the stellar system scene for saves and loads
public class V1_StellarSystemSceneDataManager : V1_SceneDataManager
{
	//public static V1_StellarSystemSceneDataManager instance { get; private set; }
	public static V1_SceneDataManager instance { get; private set; }
	public V1_StellarSystemSaveData saveData;
	private V1_GameSaveDataManager gsdm;
	public List<IStellarSystemSavable> savables;

	private void Awake()
	{
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	private void Start()
	{
		if (V1_GameSaveDataManager.instance == null)
		{
			Debug.LogWarning("V1_GameSaveDataManager not found! Returning to menu");
			SceneManager.LoadScene("V1_SCENE_MainMenu", LoadSceneMode.Single);
			return;
		}

		gsdm = V1_GameSaveDataManager.instance;
		gsdm.sdm = this;

		if (gsdm.newGame)
		{
			NewGame();
		}
		else
		{
			LoadSceneData();
		}
	}

	public void NewGame()
	{
		saveData = new V1_StellarSystemSaveData(gsdm.genericSaveData);
		// prompt the stellar system to make up new random star and planets
	}

	public override void LoadSceneData()
	{
		saveData = V1_FileHandler.Load<V1_StellarSystemSaveData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + gsdm.genericSaveData.name + ".save");
		savables = FindSavableObjects();
		foreach (var savable in savables)
		{
			// prompt them to load their data
		}
	}

	public override void SaveSceneData()
	{
		gsdm.genericSaveData.dateModified = DateTime.Now.ToString("yyyy-MM-dd");
		saveData = new V1_StellarSystemSaveData(gsdm.genericSaveData);
		savables = FindSavableObjects();
		foreach (var savable in savables)
		{
			// prompt them to save their data
		}
		V1_FileHandler.Save(saveData, Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + saveData.name + ".save");
	}

	private List<IStellarSystemSavable> FindSavableObjects()
	{
		IEnumerable<IStellarSystemSavable> objects = FindObjectsOfType<MonoBehaviour>().OfType<IStellarSystemSavable>();
		return new List<IStellarSystemSavable>(objects);
	}
}