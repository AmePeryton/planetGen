using System;
using UnityEngine;

// Handles and stores game settings and config details
// Does not handle UI elements or save files
public class V1_SettingsManager : MonoBehaviour
{
	public static V1_SettingsManager instance { get; private set; }
	public V1_SettingsData data;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
		LoadSettingsData();
	}

	// Create new settings data
	public void NewSettingsData()
	{
		data = new V1_SettingsData();
	}

	// Load settings from file
	public void LoadSettingsData()
	{
		data = V1_FileHandler.Load<V1_SettingsData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles/settings.cfg");
		if (data == null)
		{
			Debug.LogWarning("Settings file not found! Defaulting data");
			NewSettingsData();
			SaveSettingsData();
		}
	}

	// Save settings to a file
	public void SaveSettingsData()
	{
		if (data != null)
		{
			V1_FileHandler.Save(data, Application.dataPath + "/Gameplay Beta/V1_GameFiles/settings.cfg");
		}
		else
		{
			Debug.Log("Null Settings Data!");
		}
	}
}

// The actual settings data to be saved
[Serializable]
public class V1_SettingsData
{
	public float volume;

	public V1_SettingsData()
	{
		volume = 1.0f;
	}
}