public interface ISettingsSavable
{
	void LoadSettingsData(V1_SettingsData data);
	void SaveSettingsData(ref V1_SettingsData data);
}