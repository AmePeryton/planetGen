using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Rendering;

// Handles and stores game settings and config details
// Does not handle UI elements or save files
public class V1_SettingsManager : MonoBehaviour
{
	public static V1_SettingsManager instance { get; private set; }
	public List<ISettingsSavable> savableObjects;
	public V1_SettingsData settingsData;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if (instance != null)
		{
			Debug.LogError("V1_SettingsManager already present in scene!");
		}
		instance = this;
		LoadSettings();
	}

	void Start()
	{
	}

	void Update()
	{
	}

	public void NewSettings()
	{
		settingsData = new V1_SettingsData();
		SaveSettings();
	}

	[ContextMenu("LoadSettings")]
	public void LoadSettings()
	{
		this.settingsData = V1_FileHandler.Load<V1_SettingsData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles", "settings", "cfg");
		this.savableObjects = FindAllSettingsSavableObjects();
		if (this.settingsData == null)
		{
			Debug.Log("Settings data defaulted");
			NewSettings();
		}

		foreach (ISettingsSavable savedObject in savableObjects)
		{
			savedObject.LoadData(settingsData);
		}
	}

	[ContextMenu("SaveSettings")]
	public void SaveSettings()
	{
		this.savableObjects = FindAllSettingsSavableObjects();
		foreach (ISettingsSavable savedObject in savableObjects)
		{
			savedObject.SaveData(ref settingsData);
		}

		V1_FileHandler.Save(settingsData, Application.dataPath + "/Gameplay Beta/V1_GameFiles", "settings", "cfg");
	}

	private List<ISettingsSavable> FindAllSettingsSavableObjects()
	{
		IEnumerable<ISettingsSavable> objects = FindObjectsOfType<MonoBehaviour>().OfType<ISettingsSavable>();
		return new List<ISettingsSavable>(objects);
	}
}

// The actual settings data to be saved
[System.Serializable]
public class V1_SettingsData
{
	public float volume;

	public V1_SettingsData()
	{
		volume = 1.0f;
	}
}