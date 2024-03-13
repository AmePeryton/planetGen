using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class V1_LoadGameListItem : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dateCreatedText;
	public TextMeshProUGUI dateModifiedText;
	public string name;
	public string dateCreated;
	public string dateModified;
	public int gameState;

	private void Awake()
	{
		
	}

	public void Instantiate(string name, string dateCreated, string dateModified)
	{
		this.name = name;
		this.dateCreated = dateCreated;
		this.dateModified = dateModified;

		nameText.text = name;
		dateCreatedText.text = dateCreated;
		dateModifiedText.text = dateModified;
	}

	public void Load()
	{
		V1_GameSaveDataManager.instance.LoadGame(name);
		SceneManager.LoadScene("V1_SCENE_StellarSystem", LoadSceneMode.Single);
	}

	public void Delete()
	{
		V1_FileHandler.DeleteFile(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + name + ".save");
		V1_LoadMenuController.instance.UpdateList();
	}
}