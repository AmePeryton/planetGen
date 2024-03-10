using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_LevelManagerMenu : MonoBehaviour, IGameSavableData
{
	public static V1_LevelManagerMenu instance { get; private set; }
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("V1_LevelManagerMenu already present in scene!");
		}
		instance = this;
	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	public void LoadData(V1_GameSaveData data)
	{
		throw new System.NotImplementedException();
	}

	public void SaveData(ref V1_GameSaveData data)
	{
		data.name = "";
	}

}