using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavableData
{
	void LoadData(V1_SaveData data);

	void SaveData(ref V1_SaveData data);
}