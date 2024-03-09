using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class V1_SaveDataManager : MonoBehaviour
{
	[Header("File Storage Config")]
	public string fileName;
	private V1_FileDataHandler dataHandler;

	public static V1_SaveDataManager instance { get; private set; }
	public V1_SaveData data;
	public List<ISavableData> savableObjects;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("V1_SaveDataManager already present in scene!");
		}
		instance = this;
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
		this.dataHandler = new V1_FileDataHandler("C:\\Users\\ameli\\Desktop\\", fileName);
		this.savableObjects = FindAllSavableObjects();
	}

	private void Update()
	{
	}

	public void NewGame()
	{
		this.data = new V1_SaveData();
	}

	[ContextMenu("LoadGame")]
	public void LoadGame()
	{
		this.data = dataHandler.Load();
		this.savableObjects = FindAllSavableObjects();
		if (this.data == null)
		{
			Debug.Log("Load data defaulted");
			NewGame();
		}

		foreach (ISavableData savedObject in savableObjects)
		{
			savedObject.LoadData(data);
		}
	}

	[ContextMenu("SaveGame")]
	public void SaveGame()
	{
		data.dateModified = DateTime.Now.ToString("yyyy-MM-dd");
		this.savableObjects = FindAllSavableObjects();
		foreach (ISavableData savedObject in savableObjects)
		{
			savedObject.SaveData(ref data);
		}

		dataHandler.Save(data);
	}

	private List<ISavableData> FindAllSavableObjects()
	{
		IEnumerable<ISavableData> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavableData>();
		return new List<ISavableData>(dataPersistenceObjects);
	}
}