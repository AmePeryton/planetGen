using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V1_LoadMenuController : MonoBehaviour
{
	public static V1_LoadMenuController instance { get; private set; }
	public GameObject contentPanel;
	public GameObject listItemPrefab;
	public List<V1_GameSaveData> dataList;
	public List<GameObject> listItems;

	private RectTransform rect;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("V1_SettingsManager already present in scene!");
		}
		instance = this;
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
		dataList.Clear();

		rect.sizeDelta = new Vector2(rect.sizeDelta.x, 40);
		string[] paths = V1_FileHandler.FindAllFiles(Application.dataPath + "/Gameplay Beta/V1_GameFiles", "save");
		foreach (string path in paths)
		{
			dataList.Add(V1_FileHandler.Load<V1_GameSaveData>(path));
		}

		dataList = dataList.OrderByDescending(x => x.dateModified).ThenBy(y => y.name).ToList();

		foreach (V1_GameSaveData data in dataList)
		{
			rect.sizeDelta += new Vector2(0, 280);
			GameObject newListItem = Instantiate(listItemPrefab);
			newListItem.transform.SetParent(contentPanel.transform);
			RectTransform newRect = newListItem.GetComponent<RectTransform>();
			newRect.anchoredPosition = new Vector2(20, 300 - rect.sizeDelta.y);
			newRect.sizeDelta = new Vector2(1240, 240);
			newListItem.GetComponent<V1_LoadGameListItem>().Instantiate(data.name, data.dateCreated, data.dateModified);
			listItems.Add(newListItem);
		}
	}
}