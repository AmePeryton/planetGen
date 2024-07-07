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
		if (V1_SaveDataManager.instance.data == null)
		{
			Debug.LogWarning("Save file " + fileName + " not found!");
			return;
		}

		switch (V1_SaveDataManager.instance.data.common.gameScene)
		{
			case GameScene.MainMenu:
				Debug.LogWarning("Invalid Data: MainMenu gameScene");
				break;
			case GameScene.StellarSystem:
				SceneManager.LoadScene("V1_SCENE_StellarSystem", LoadSceneMode.Single);
				break;
			case GameScene.AncestorSelection:
				Debug.LogWarning("Unimplemented: AncestorSelection gameScene");
				break;
			case GameScene.CreatureCreator:
				Debug.LogWarning("Unimplemented: CreatureCreator gameScene");
				break;
			case GameScene.OpenWorld:
				Debug.LogWarning("Unimplemented: OpenWorld gameScene");
				break;
			default:
				Debug.LogWarning("Unimplemented: DEFAULT gameScene");
				break;
		}
	}
}