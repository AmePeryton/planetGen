public interface ISettingsSavable
{
	void LoadData(V1_SettingsData data);
	void SaveData(ref V1_SettingsData data);
}