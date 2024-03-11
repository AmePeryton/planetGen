using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class V1_PlayPanelController : MonoBehaviour, IGameSavableData
{
	public GameObject newSavePanel;
	public TMP_InputField saveNameInput;

	private void Awake()
	{
		CloseNewSavePanel();
	}

	public void OpenNewSavePanel()
	{
		newSavePanel.SetActive(true);
	}

	public void CloseNewSavePanel()
	{
		saveNameInput.text = string.Empty;
		newSavePanel.SetActive(false);
	}

	public void NewSave()
	{
		// CREATE NEW DEFAULT SAVEFILE
		// GO TO STELLAR LEVEL
		V1_GameSaveDataManager.instance.SaveGame();
		SceneManager.LoadScene("V1_SCENE_StellarSystem", LoadSceneMode.Single);
	}

	public void LoadGameSaveData(V1_GameSaveData data)
	{
	}

	public void SaveGameSaveData(ref V1_GameSaveData data)
	{
		if (saveNameInput.text.Length > 0)
		{
			data.name = saveNameInput.text;
		}
		else
		{
			data.name = "save";
		}
		data.dateCreated = DateTime.Now.ToString("yyyy-MM-dd");
	}
}