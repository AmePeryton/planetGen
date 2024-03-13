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

	public void LoadGame(string saveName)
	{
		this.saveData = V1_FileHandler.Load<V1_GameSaveData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + saveName + ".save");
		this.savableObjects = FindAllGameSavableObjects();
		if (this.saveData == null)
		{
			Debug.Log("Load data defaulted");
			NewGame();
		}

		foreach (IGameSavableData savedObject in savableObjects)
		{
			savedObject.LoadGameSaveData(saveData);
		}
	}

	[ContextMenu("SaveGame")]
	public void SaveGame()
	{
		this.savableObjects = FindAllGameSavableObjects();
		foreach (IGameSavableData savedObject in savableObjects)
		{
			savedObject.SaveGameSaveData(ref saveData);
		}
		saveData.dateModified = DateTime.Now.ToString("yyyy-MM-dd");

		V1_FileHandler.Save(saveData, Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + saveData.name + ".save");
	}

	private List<IGameSavableData> FindAllGameSavableObjects()
	{
		IEnumerable<IGameSavableData> objects = FindObjectsOfType<MonoBehaviour>().OfType<IGameSavableData>();
		return new List<IGameSavableData>(objects);
	}
}

[System.Serializable]
public class V1_GameSaveData
{
	// Standard Data
	public string name;
	public string dateCreated;
	public string dateModified;
	public GameState gameState;

	// Stellar Object Data
	public StarData starData;
	public PlanetData planetData;

	public enum GameState
	{
		Menu = 0,           // Game saved while in main menu, before stellar system loaded
		Stellar,            // Game saved while tweaking the Stellar System or the Planet
		LCA,                // Game saved while choosing a LCA
		CreatureCreator,    // Game saved while tweaking your organism (who up tweaking they organism) or in physical simulaton
		Gameplay            // Game saved during open world gameplay
	}

	public V1_GameSaveData()
	{
		name = "DEFAULT NAME";
		dateCreated = "1970-01-01";
		dateModified = "1970-01-01";
		gameState = GameState.Menu;
		starData = new StarData();
		planetData = new PlanetData();
	}
}

[System.Serializable]
public class StarData
{
	public float mass;
	public float age;

	public StarData()
	{
		mass = 0;
		age = 0;
	}
}

[System.Serializable]
public class PlanetData
{
	public float mass;
	public float radius;
	public float distance;

	public PlanetData()
	{
		mass = 0;
		radius = 0;
		distance = 0;
	}
}