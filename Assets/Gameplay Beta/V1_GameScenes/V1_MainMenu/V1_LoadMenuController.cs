using System.Collections.Generic;
using UnityEngine;

public class V1_LoadMenuController : MonoBehaviour
{
	public static V1_LoadMenuController instance { get; private set; }
	public GameObject contentPanel;
	public GameObject listItemPrefab;
	public List<GameObject> listItems;

	private RectTransform rect;

	private void Awake()
	{
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
		rect = contentPanel.GetComponent<RectTransform>();
	}

	private void OnEnable()
	{
		UpdateList();
	}

	public void UpdateList()
	{
		foreach (GameObject obj in listItems)
		{
			Destroy(obj);
		}
		listItems.Clear();

		rect.sizeDelta = new Vector2(rect.sizeDelta.x, 40);
		string[] paths = V1_FileHandler.FindAllFiles(Application.dataPath + "/Gameplay Beta/V1_GameFiles", "save");

		//paths = paths.OrderBy(x => x.ToString()).ToArray();

		// Verify files and spawn load list items
		foreach (string path in paths)
		{
			V1_FullSaveData data = V1_FileHandler.Load<V1_FullSaveData>(path);
			if (data == null)
			{
				Debug.LogWarning("Bad data in save file " + V1_FileHandler.GetFileName(path) + ".save");
				continue;
			}

			// If name inside json data is different from the file name, rename the json data to match the file name
			if (data.common.name != V1_FileHandler.GetFileName(path))
			{
				Debug.LogWarning("Save file name mismatch: the file \"" + V1_FileHandler.GetFileName(path) + ".save\" contains data for game \"" + 
					data.common.name + "\". Renaming game to \"" + V1_FileHandler.GetFileName(path) + "\"");
				data.common.name = V1_FileHandler.GetFileName(path);
				V1_FileHandler.Save(data, path);
			}

			// TODO: make this a unity list
			// Spawn load list element
			rect.sizeDelta += new Vector2(0, 280);
			GameObject newListItem = Instantiate(listItemPrefab);
			newListItem.transform.SetParent(contentPanel.transform);
			RectTransform newRect = newListItem.GetComponent<RectTransform>();
			newRect.anchoredPosition = new Vector2(20, 300 - rect.sizeDelta.y);
			newRect.sizeDelta = new Vector2(1240, 240);
			newListItem.GetComponent<V1_LoadGameListItem>().commonData = data.common;
			newListItem.GetComponent<V1_LoadGameListItem>().Instantiate();
			listItems.Add(newListItem);
		}

		//foreach (string path in paths)
		//{
		//	rect.sizeDelta += new Vector2(0, 280);
		//	GameObject newListItem = Instantiate(listItemPrefab);
		//	newListItem.transform.SetParent(contentPanel.transform);
		//	RectTransform newRect = newListItem.GetComponent<RectTransform>();
		//	newRect.anchoredPosition = new Vector2(20, 300 - rect.sizeDelta.y);
		//	newRect.sizeDelta = new Vector2(1240, 240);
		//	newListItem.GetComponent<V1_LoadGameListItem>().commonData = V1_FileHandler.Load<V1_FullSaveData>(path).common;
		//	//newListItem.GetComponent<V1_LoadGameListItem>().Instantiate(data.name, data.dateCreated, data.dateModified);
		//	newListItem.GetComponent<V1_LoadGameListItem>().Instantiate();
		//	listItems.Add(newListItem);
		//}
	}
}