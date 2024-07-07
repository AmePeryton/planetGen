using System;
using System.Collections.Generic;
using UnityEngine;

public class V1_SaveDataManager : MonoBehaviour
{
	public static V1_SaveDataManager instance { get; private set; }
	public V1_FullSaveData data;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
		// Start with default data (in the case of out of order scene loading)
		DefaultData();
	}

	// Create blank data
	public void NewData(string name)
	{
		data = new V1_FullSaveData(name);
	}

	// Create default data
	public void DefaultData()
	{
		data = new V1_FullSaveData();
	}

	// Load data from a file
	public void LoadData(string name)
	{
		data = V1_FileHandler.Load<V1_FullSaveData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + name + ".save");
	}

	// Save data to a file
	public void SaveData()
	{
		if (data != null)
		{
			data.common.dateModified = DateTime.Now.ToString("MM/dd/yyyy");
			V1_FileHandler.Save(data, Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + data.common.name + ".save");
		}
		else
		{
			Debug.Log("Null Save Data!");
		}
	}
}

[Serializable]
public class V1_FullSaveData
{
	// Common data
	public V1_SaveData_Common common;

	// Persistent data sections
	public V1_SaveData_StellarSystem stellarSystem;
	public V1_SaveData_Planet planet;
	public V1_SaveData_Species species;
	public V1_SaveData_Cladistics cladistics;
	public V1_SaveData_FoodWeb foodWeb;
	public V1_SaveData_HistoryGraph historyGraph;

	// Game scene specific data
	//public V1_GameSceneData gameSceneData;
	public V1_GameSceneData_StellarSystem gsd_stellarSystem;
	public V1_GameSceneData_AncestorSelection gsd_ancestorSelection;
	public V1_GameSceneData_CreatureCreator gsd_creatureCreator;
	public V1_GameSceneData_OpenWorld gsd_openWorld;

	// Default data (out of order game opening)
	public V1_FullSaveData()
	{
		common = new V1_SaveData_Common("DEFAULT");

		stellarSystem = new V1_SaveData_StellarSystem();
		planet = new V1_SaveData_Planet();
		species = new V1_SaveData_Species();
		cladistics = new V1_SaveData_Cladistics();
		foodWeb = new V1_SaveData_FoodWeb();
		historyGraph = new V1_SaveData_HistoryGraph();

		gsd_stellarSystem = new V1_GameSceneData_StellarSystem();
		gsd_ancestorSelection = new V1_GameSceneData_AncestorSelection();
		gsd_creatureCreator = new V1_GameSceneData_CreatureCreator();
		gsd_openWorld = new V1_GameSceneData_OpenWorld();
	}

	// Blank data (normal new game)
	public V1_FullSaveData(string saveName)
	{
		common = new V1_SaveData_Common(saveName);

		stellarSystem = new V1_SaveData_StellarSystem();
		planet = new V1_SaveData_Planet();
		species = new V1_SaveData_Species();
		cladistics = new V1_SaveData_Cladistics();
		foodWeb = new V1_SaveData_FoodWeb();
		historyGraph = new V1_SaveData_HistoryGraph();

		gsd_stellarSystem = new V1_GameSceneData_StellarSystem();
		gsd_ancestorSelection = new V1_GameSceneData_AncestorSelection();
		gsd_creatureCreator = new V1_GameSceneData_CreatureCreator();
		gsd_openWorld = new V1_GameSceneData_OpenWorld();
	}
}

[Serializable]
public class V1_SaveData_Common
{
	public string name;
	public string dateCreated;
	public string dateModified;
	public GameScene gameScene;

	public V1_SaveData_Common(string name)
	{
		this.name = name;
		dateCreated = DateTime.Now.ToString("MM/dd/yyyy");
		dateModified = DateTime.Now.ToString("MM/dd/yyyy");
		gameScene = GameScene.MainMenu;
	}
}

// PERSISTENT DATA TYPES
[Serializable]
public class V1_SaveData_StellarSystem
{
	public StarData starData;
	public MainPlanetData mainPlanetData;
	public List<OtherPlanetData> otherPlanetsData;

	public V1_SaveData_StellarSystem()
	{
		starData = new StarData();
		mainPlanetData = new MainPlanetData();
		otherPlanetsData = new List<OtherPlanetData>();
	}
}

[Serializable]
public class V1_SaveData_Planet
{
	public V1_SaveData_Planet()
	{
	}
}

[Serializable]
public class V1_SaveData_Species
{
	public V1_SaveData_Species()
	{
	}
}

[Serializable]
public class V1_SaveData_Cladistics
{
	public V1_SaveData_Cladistics()
	{
	}
}

[Serializable]
public class V1_SaveData_FoodWeb
{
	public V1_SaveData_FoodWeb()
	{
	}
}

[Serializable]
public class V1_SaveData_HistoryGraph
{
	public V1_SaveData_HistoryGraph()
	{
	}
}

// GAME SCENE SPECIFIC DATA TYPES
[Serializable]
public class V1_GameSceneData
{
	public V1_GameSceneData()
	{
	}
}

// ENUMS
[Serializable]
public enum GameScene
{
	MainMenu = 0,		// Default game scene, indicates new game or out of order loading
	StellarSystem,		// Game saved while tweaking the stellar system or the planet
	AncestorSelection,	// Game saved while choosing a last common ancestor
	CreatureCreator,	// Game saved while tweaking your organism (who up tweaking they organism) or in physical simulaton
	OpenWorld			// Game saved during open world gameplay
}