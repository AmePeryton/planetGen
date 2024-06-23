using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class V1_LoadGameListItem : MonoBehaviour
{
	public V1_SaveData_Common commonData;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dateCreatedText;
	public TextMeshProUGUI dateModifiedText;

	public void Load()
	{
		// Pass to main menu controller for organizational reasons
		V1_MainMenuController.instance.LoadGame(commonData.name);
	}

	public void Delete()
	{
		V1_FileHandler.DeleteFile(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + commonData.name + ".save");
		V1_LoadMenuController.instance.UpdateList();
	}

	public void Instantiate()
	{
		nameText.text = commonData.name;
		dateCreatedText.text = commonData.dateCreated;
		dateModifiedText.text = commonData.dateModified;
	}
}