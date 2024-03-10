using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

// Reads from and writes to JSON files, converting to or from a C# class
public class V1_FileHandler
{
	// Retrieve and deserialize JSON data to C# class data of type <T>
	public static T Load<T>(string path, string fileName, string extension = "json")
	{
		// Get full path
		string fullPath = Path.Combine(path, fileName + "." + extension);
		T loadedData = default;

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

				// Deserialize JSON formatted string to C# data of type <T>
				loadedData = JsonUtility.FromJson<T>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
			}
		}

		Debug.Log("Data loaded from file " + fullPath);
		return loadedData;
	}

	// Serialize C# data of type <T> and write JSON formatted string to file
	public static void Save<T>(T data, string path, string fileName, string extension = "json")
	{
		// Get full path
		string fullPath = Path.Combine(path, fileName + "." + extension);

		try
		{
			// Create directory
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			// Serialize C# data of type <T> to JSON
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
			Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
		}
		Debug.Log("Data saved to file " + fullPath);
	}
}