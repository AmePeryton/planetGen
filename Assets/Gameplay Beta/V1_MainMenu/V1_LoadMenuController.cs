using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		foreach (string path in paths)
		{
			//dataList.Add(V1_FileHandler.Load<V1_GameSaveData>(path));
			rect.sizeDelta += new Vector2(0, 280);
			GameObject newListItem = Instantiate(listItemPrefab);
			newListItem.transform.SetParent(contentPanel.transform);
			RectTransform newRect = newListItem.GetComponent<RectTransform>();
			newRect.anchoredPosition = new Vector2(20, 300 - rect.sizeDelta.y);
			newRect.sizeDelta = new Vector2(1240, 240);
			newListItem.GetComponent<V1_LoadGameListItem>().commonData = V1_FileHandler.Load<V1_FullSaveData>(path).common;
			// TODO: accomodate invalid files in folder and misnamed save files
			//newListItem.GetComponent<V1_LoadGameListItem>().Instantiate(data.name, data.dateCreated, data.dateModified);
			newListItem.GetComponent<V1_LoadGameListItem>().Instantiate();
			listItems.Add(newListItem);
		}
	}
}