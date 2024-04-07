using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_SceneController : MonoBehaviour
{
	public static V1_SceneController instance { get; private set; }

	protected void SceneControllerInit()
	{
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	public virtual void LoadData() { } // Load data from a file to the scene
	public virtual void SaveData() { } // Save the scene's data to a file//
}