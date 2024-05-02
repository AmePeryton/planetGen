using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class V1_GameSaveDataManager : MonoBehaviour
{
	public static V1_GameSaveDataManager instance { get; private set; }
	public V1_GameSaveData genericSaveData;
	public bool newGame;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	public void NewGame(string name)
	{
		genericSaveData = new V1_GameSaveData(name);
		newGame = true;
		SceneManager.LoadScene("V1_SCENE_StellarSystem", LoadSceneMode.Single);
	}

	public void LoadGame(string saveName)
	{
		newGame = false;
		genericSaveData = V1_FileHandler.Load<V1_GameSaveData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + saveName + ".save");

		switch (genericSaveData.gameState)
		{
			case GameState.MainMenu:
				SceneManager.LoadScene("V1_SCENE_MainMenu", LoadSceneMode.Single);
				break;
			case GameState.StellarSystem:
				SceneManager.LoadScene("V1_SCENE_StellarSystem", LoadSceneMode.Single);
				break;
			case GameState.LCASelection:
				SceneManager.LoadScene("V1_SCENE_LCASelection", LoadSceneMode.Single);
				break;
			case GameState.CreatureCreator:
				SceneManager.LoadScene("V1_SCENE_CreatureCreator", LoadSceneMode.Single);
				break;
			case GameState.Gameplay:
				SceneManager.LoadScene("V1_SCENE_Gameplay", LoadSceneMode.Single);
				break;
			default:
				break;
		}
	}
}

[Serializable]
public enum GameState
{
	MainMenu = 0,		// Game saved while in main menu, before stellar system loaded
	StellarSystem,		// Game saved while tweaking the Stellar System or the Planet
	LCASelection,		// Game saved while choosing a LCA
	CreatureCreator,	// Game saved while tweaking your organism (who up tweaking they organism) or in physical simulaton
	Gameplay			// Game saved during open world gameplay
}

[Serializable]
public class V1_GameSaveData
{
	public string name;
	public string dateCreated;
	public string dateModified;
	public GameState gameState;

	public V1_GameSaveData(string name)
	{
		this.name = name;
		dateCreated = DateTime.Now.ToString("yyyy-MM-dd");
		dateModified = DateTime.Now.ToString("yyyy-MM-dd");
		gameState = GameState.MainMenu;
	}
}

[Serializable]
public class MenuSceneData : V1_GameSaveData
{
	// deprecated, should not be saved or loaded in this state
	// if this is the gamestate in a file, something has gone wrong
	public MenuSceneData(V1_GameSaveData generic) : base(generic.name)
	{
		//name = generic.name;
		dateCreated = generic.dateCreated;
		dateModified = generic.dateModified;
		gameState = GameState.MainMenu;
	}
}

[Serializable]
public class LastCommonAncestorSceneData : V1_GameSaveData
{
	public int playerSelection;

	public LastCommonAncestorSceneData(V1_GameSaveData generic) : base(generic.name)
	{
		//name = generic.name;
		dateCreated = generic.dateCreated;
		dateModified = generic.dateModified;
		gameState = GameState.LCASelection;
	}
}

[Serializable]
public class CreatureCreatorSceneData : V1_GameSaveData
{
	public float zoom;

	public CreatureCreatorSceneData(V1_GameSaveData generic) : base(generic.name)
	{
		//name = generic.name;
		dateCreated = generic.dateCreated;
		dateModified = generic.dateModified;
		gameState = GameState.CreatureCreator;
	}
}

[Serializable]
public class GameplaySceneData : V1_GameSaveData
{
	public float temperature;

	public GameplaySceneData(V1_GameSaveData generic) : base(generic.name)
	{
		//name = generic.name;
		dateCreated = generic.dateCreated;
		dateModified = generic.dateModified;
		gameState = GameState.Gameplay;
	}
}