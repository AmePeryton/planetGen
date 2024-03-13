using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

// Deals with file related functions, inclusind:
	// Serialization / deserialization
	// Creating / Deleting files
	// Finding all relevant files in a directory
public class V1_FileHandler
{
	// Retrieve and deserialize JSON data to C# class data of type <T>
	public static T Load<T>(string fullPath)
	{
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
		else
		{
			Debug.LogError("[LOAD] File could not be found at " + fullPath);
		}

		Debug.Log("Data loaded from file " + fullPath);
		return loadedData;
	}

	// Serialize C# data of type <T> and write JSON formatted string to file
	public static void Save<T>(T data, string fullPath)
	{
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

	// Delete a file from a given folder (intended to delete game save files)
	public static void DeleteFile(string fullPath)
	{
		if (File.Exists(fullPath))
		{
			try
			{
				File.Delete(fullPath);
			}
			catch (Exception e)
			{
				Debug.LogError("Error occured when trying to delete file: " + fullPath + "\n" + e);
			}
		}
		else
		{
			Debug.LogError("[DELETE] File could not be found at " + fullPath);
		}

		Debug.Log("File deleted: " + fullPath);
	}

	public static string[] FindAllFiles(string directory, string extension = "*")
	{
		List<string> paths = new List<string>();
		DirectoryInfo info = new DirectoryInfo(directory);
		FileInfo[] files = info.GetFiles("*." + extension);
		foreach (FileInfo file in files)
		{
			paths.Add(file.FullName);
			Debug.Log("FILE: " + file.FullName);
		}
		return paths.ToArray();
	}
}