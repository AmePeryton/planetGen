using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class V1_GameSaveDataManager : MonoBehaviour
{
	public static V1_GameSaveDataManager instance { get; private set; }
	public List<IGameSavableData> savableObjects;
	public V1_GameSaveData saveData;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if (instance != null)
		{
			Debug.LogError("V1_SaveDataManager already present in scene!");
		}
		instance = this;
	}

	void Start()
	{
	}

	void Update()
	{
	}

	public void NewGame()
	{
		this.saveData = new V1_GameSaveData();
	}

	[ContextMenu("LoadGame")]
	public void LoadGame()
	{
		this.saveData = V1_FileHandler.Load<V1_GameSaveData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles", saveData.name, "save");
		this.savableObjects = FindAllGameSavableObjects();
		if (this.saveData == null)
		{
			Debug.Log("Load data defaulted");
			NewGame();
		}

		foreach (IGameSavableData savedObject in savableObjects)
		{
			savedObject.LoadData(saveData);
		}
	}

	[ContextMenu("SaveGame")]
	public void SaveGame()
	{
		this.savableObjects = FindAllGameSavableObjects();
		foreach (IGameSavableData savedObject in savableObjects)
		{
			savedObject.SaveData(ref saveData);
		}
		saveData.dateModified = DateTime.Now.ToString("yyyy-MM-dd");

		V1_FileHandler.Save(saveData, Application.dataPath + "/Gameplay Beta/V1_GameFiles", saveData.name, "save");
	}

	private List<IGameSavableData> FindAllGameSavableObjects()
	{
		IEnumerable<IGameSavableData> objects = FindObjectsOfType<MonoBehaviour>().OfType<IGameSavableData>();
		return new List<IGameSavableData>(objects);
	}
}