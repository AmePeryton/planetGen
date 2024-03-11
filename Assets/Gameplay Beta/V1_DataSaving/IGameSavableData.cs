public interface IGameSavableData
{
	void LoadData(V1_GameSaveData data);
	void SaveData(ref V1_GameSaveData data);
}