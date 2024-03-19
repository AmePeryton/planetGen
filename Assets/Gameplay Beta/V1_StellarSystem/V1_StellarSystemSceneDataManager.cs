using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages all gameobjects in the stellar system scene for saves and loads
public class V1_StellarSystemSceneDataManager : V1_SceneDataManager
{
	//public static V1_StellarSystemSceneDataManager instance { get; private set; }
	public static V1_SceneDataManager instance { get; private set; }
	public V1_StellarSystemSaveData saveData;
	public List<V1_Star> stars;
	public List<V1_Planet> planets;
	private V1_GameSaveDataManager gsdm;

	private void Awake()
	{
		// Singleton line
		if (instance != null) { Debug.LogError(GetType().Name + " already present in scene!"); } instance = this;

		gsdm = V1_GameSaveDataManager.instance;
		V1_GameSaveDataManager.instance.sdm = this;

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
		saveData = new V1_StellarSystemSaveData(gsdm.genericSaveData.name);
		Debug.Log("Stellar scene manager: New game!");
	}

	public override void LoadSceneData()
	{
		saveData = V1_FileHandler.Load<V1_StellarSystemSaveData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + gsdm.genericSaveData.name + ".save");
		Debug.Log("Stellar scene manager: Loaded game!");
	}

	public override void SaveSceneData()
	{
		saveData.dateModified = DateTime.Now.ToString("yyyy-MM-dd");

		V1_FileHandler.Save(saveData, Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + saveData.name + ".save");
	}
}