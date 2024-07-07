using System;
using UnityEngine;

public class V1_OpenWorldController : V1_SceneController
{
	public static new V1_OpenWorldController instance { get; private set; }

	private void Awake()
	{
		SceneControllerInit();
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	void Start()
	{
		
	}

	public override void LoadScene()
	{

	}

	public override void NewScene()
	{

	}

	public override void SaveScene()
	{

	}
}

[Serializable]
public class V1_GameSceneData_OpenWorld : V1_GameSceneData
{
	public V1_GameSceneData_OpenWorld()
	{
	}
}