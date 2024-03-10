using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameSavableData
{
	void LoadData(V1_GameSaveData data);
	void SaveData(ref V1_GameSaveData data);
}