using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_LoadMenuController : MonoBehaviour
{
	public GameObject contentPanel;
	public GameObject listItemPrefab;
	public List<V1_GameSaveData> dataList;
	public List<GameObject> listItems;

	private void Awake()
	{
		// find all .save files in folder
		// extract data from them as list of gamesavedatas
		// order list by modified date, descending
		// foreach gamesavedata, spawn new prefab with the relevant data and add height to the content panel
	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}
}