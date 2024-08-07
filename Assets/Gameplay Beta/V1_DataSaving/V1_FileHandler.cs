using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Deals with file related functions, including:
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
				Debug.LogWarning("Error occured when trying to load data from file: " + fullPath + "\n" + e);
			}
		}
		else
		{
			Debug.LogWarning("File could not be found at " + fullPath);
		}

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
			Debug.LogWarning("Error occured when trying to save data to file: " + fullPath + "\n" + e);
		}
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
				Debug.LogWarning("Error occured when trying to delete file: " + fullPath + "\n" + e);
			}
		}
		else
		{
			Debug.LogWarning("File could not be found at " + fullPath);
		}
	}

	// Get name of a file (without extension) from its path
	public static string GetFileName(string path)
	{
		// Even though it's one line, it prevents adding extra IO usings to files that dont need them
		return Path.GetFileNameWithoutExtension(path);
	}

	// Find all files in a directory, return full paths of each file
	public static string[] FindAllFiles(string directory, string extension = "*")
	{
		List<string> paths = new List<string>();
		DirectoryInfo info = new DirectoryInfo(directory);
		FileInfo[] files = info.GetFiles("*." + extension);
		foreach (FileInfo file in files)
		{
			paths.Add(file.FullName);
		}
		return paths.ToArray();
	}
}