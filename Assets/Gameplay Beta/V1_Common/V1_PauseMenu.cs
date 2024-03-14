using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class V1_PauseMenu : MonoBehaviour
{
	public GameObject background;

	private void Awake()
	{
		background.SetActive(false);
	}

	private void OnGUI()
	{
		if (Event.current.Equals(Event.KeyboardEvent("[esc]")))
		{
			background.SetActive(!background.activeSelf);
		}
	}

	public void Resume()
	{
		background.SetActive(false);
	}

	public void SaveGame()
	{
		V1_GameSaveDataManager.instance.SaveGame();
	}

	public void MainMenu()
	{
		SceneManager.LoadScene("V1_SCENE_MainMenu", LoadSceneMode.Single);
	}

	public void Quit()
	{
		Application.Quit();
	}
}