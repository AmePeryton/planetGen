using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsSavable
{
	void LoadData(V1_SettingsData data);
	void SaveData(ref V1_SettingsData data);
}