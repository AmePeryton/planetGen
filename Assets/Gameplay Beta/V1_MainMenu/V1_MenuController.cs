using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controller for all menu panels in the the Main Menu scene
public class V1_MenuController : MonoBehaviour, ISavableData
{
	public GameObject mainPanel;
	public GameObject playPanel;
	public GameObject settingsPanel;
	public GameObject newSavePanel;
	public TextMeshProUGUI directoryNameText;
	public TextMeshProUGUI saveName;
	public string saveFileDirectory;

	void Start()
	{
		OpenMainPanel();
	}

	// Switch Panels
	public void OpenMainPanel()
	{
		mainPanel.SetActive(true);
		playPanel.SetActive(false);
		settingsPanel.SetActive(false);
		newSavePanel.SetActive(false);
	}

	public void OpenPlayPanel()
	{
		mainPanel.SetActive(false);
		playPanel.SetActive(true);
		settingsPanel.SetActive(false);
		newSavePanel.SetActive(false);
	}

	public void OpenOptionsPanel()
	{
		mainPanel.SetActive(false);
		playPanel.SetActive(false);
		settingsPanel.SetActive(true);
		newSavePanel.SetActive(false);
	}

	// Main Menu Functions
	public void BtnQuit()
	{
		Debug.Log("Quit Button Pressed");
		Application.Quit();
	}

	// Settings Menu Functions

	// Play Menu Functions
	public void OpenNewSavePanel()
	{
		newSavePanel.SetActive(true);
	}

	public void CloseNewSavePanel()
	{
		newSavePanel.SetActive(false);
	}

	public void NewSave()
	{
		// CREATE NEW DEFAULT SAVEFILE
		// GO TO STELLAR LEVEL
		SceneManager.LoadScene("V1_SCENE_StellarSystem", LoadSceneMode.Single);
	}

	public void SelectDirectory()
	{
		saveFileDirectory = StandaloneFileBrowser.OpenFolderPanel("Select Directory", "", false)[0];
		directoryNameText.text = saveFileDirectory;
	}

	public void LoadData(V1_SaveData data)
	{
	}

	public void SaveData(ref V1_SaveData data)
	{
		if (saveName.text.Length > 0)
		{
			data.name = saveName.text;
		}
		else
		{
			data.name = "save";
		}
		data.dateCreated = DateTime.Now.ToString("yyyy-MM-dd");
	}
}