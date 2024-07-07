using System;
using TMPro;
using UnityEngine;

public class V1_PlayPanelController : MonoBehaviour
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

	public void NewGame()
	{
		// Pass to main menu controller for organizational reasons
		if (saveNameInput.text != string.Empty)
		{
			V1_MainMenuController.instance.NewGame(saveNameInput.text);
		}
		else
		{
			// Give a default but unique save name if the field is empty
			V1_MainMenuController.instance.NewGame("Unnamed Save " + DateTime.Now.ToString("yyyy-MM-dd"));
		}
	}
}