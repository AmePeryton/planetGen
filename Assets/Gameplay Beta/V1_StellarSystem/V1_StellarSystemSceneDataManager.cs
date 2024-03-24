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
	//private V1_GameSaveDataManager gsdm;
	public List<IStellarSystemSavable> savables;

	private void Awake()
	{
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	private void Start()
	{
		// Return to menu if game is opened in stellar system scene
		if (V1_GameSaveDataManager.instance == null)
		{
			Debug.LogWarning("V1_GameSaveDataManager not found! Returning to menu");
			SceneManager.LoadScene("V1_SCENE_MainMenu", LoadSceneMode.Single);
			return;
		}

		//gsdm = V1_GameSaveDataManager.instance;
		V1_GameSaveDataManager.instance.sdm = this;

		if (V1_GameSaveDataManager.instance.newGame)
		{
			NewGame();
		}
		else
		{
			LoadSceneData();
		}
	}

	// Create new data without loading existing data
	public void NewGame()
	{
		saveData = new V1_StellarSystemSaveData(V1_GameSaveDataManager.instance.genericSaveData);
		V1_StellarSystem.instance.NewStellarSystem();
	}

	// Load data from a file to the scene
	public override void LoadSceneData()
	{
		saveData = V1_FileHandler.Load<V1_StellarSystemSaveData>
			(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + V1_GameSaveDataManager.instance.genericSaveData.name + ".save");
		V1_StellarSystem.instance.LoadData(saveData);
	}

	// Save the scene's data to a file
	public override void SaveSceneData()
	{
		V1_GameSaveDataManager.instance.genericSaveData.dateModified = DateTime.Now.ToString("yyyy-MM-dd");
		saveData = new V1_StellarSystemSaveData(V1_GameSaveDataManager.instance.genericSaveData);
		V1_StellarSystem.instance.SaveData(ref saveData);
		V1_FileHandler.Save(saveData, Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + saveData.name + ".save");
	}
}