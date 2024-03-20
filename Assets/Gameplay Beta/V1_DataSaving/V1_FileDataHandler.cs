using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

// File handler from youtube tutorial, only handles SaveData classes and has some annoying quirks, now obselete
[Obsolete]
// Replaced by V1_FileHandler
public class V1_FileDataHandler
{
	public string dataDirPath = "";
	public string dataFileName = "";

	public V1_FileDataHandler(string dataDirPath, string dataFileName)
	{
		this.dataDirPath = dataDirPath;
		this.dataFileName = dataFileName;
	}

	public V1_GameSaveData Load()
	{
		// Get full path
		string fullPath = Path.Combine(dataDirPath, dataFileName);
		V1_GameSaveData loadedData = null;

		if (File.Exists(fullPath))
		{
			try
			{
				// Get JSON data from file as string
				string dataToLoad = "";
				using (FileStream stream = new FileStream(fullPath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						dataToLoad = reader.ReadToEnd();
					}
				}

				// Deserialize JSON formatted string to C# data
				loadedData = JsonUtility.FromJson<V1_GameSaveData>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.LogWarning("Error occured when trying to load data from file: " + fullPath + "\n" + e);
			}
		}

		Debug.Log("Data loaded from file " + fullPath);
		return loadedData;
	}

	public void Save(V1_GameSaveData data)
	{
		// Get full path
		string fullPath = Path.Combine(dataDirPath, dataFileName);

		try
		{
			// Create directory
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			// Serialize C# data to JSON
			string dataToStore = JsonUtility.ToJson(data, true);

			// Write JSON formatted string to file
			using (FileStream stream = new FileStream(fullPath, FileMode.Create))
			{
				using (StreamWriter writer = new StreamWriter(stream))
				{
					writer.Write(dataToStore);
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning("Error occured when trying to save data to file: " + fullPath + "\n" + e);
		}
		Debug.Log("Data saved to file " + fullPath);
	}
}