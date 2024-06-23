using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class V1_MainMenuController : V1_SceneController
{
	public static new V1_MainMenuController instance { get; private set; }
	public ActivePanel activePanel;
	public List<GameObject> panels; // Title, Settings, Play

	[Serializable]
	public enum ActivePanel
	{
		Title,
		Settings,
		Play
	}

	private void Awake()
	{
		SceneControllerInit();
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
		SwitchPanel(0);
	}

	public void SwitchPanel(int panel)	// The int parameter (instead of ActivePanel) is to properly interface with unity buttons
	{
		activePanel = (ActivePanel)panel;
		foreach (GameObject p in panels)
		{
			p.SetActive(false);
		}
		panels[panel].SetActive(true);
	}

	public void NewGame(string name)	// NOTE: after adding more options to the newgame panel, make parameter the common class
	{
		V1_SaveDataManager.instance.NewData(name);
		SceneManager.LoadScene("V1_SCENE_StellarSystem", LoadSceneMode.Single);
	}

	public void LoadGame(string fileName)
	{
		V1_SaveDataManager.instance.LoadData(fileName);

		switch (V1_SaveDataManager.instance.data.common.gameState)
		{
			case GameState.MainMenu:
				Debug.LogWarning("Invalid Data: MainMenu gameState");
				break;
			case GameState.StellarSystem:
				SceneManager.LoadScene("V1_SCENE_StellarSystem", LoadSceneMode.Single);
				break;
			case GameState.AncestorSelection:
				Debug.LogWarning("Unimplemented: LCASelection gameState");
				break;
			case GameState.CreatureCreator:
				Debug.LogWarning("Unimplemented: CreatureCreator gameState");
				break;
			case GameState.OpenWorld:
				Debug.LogWarning("Unimplemented: Gameplay gameState");
				break;
			default:
				Debug.LogWarning("Unimplemented: DEFAULTED gameState");
				break;
		}
	}
}