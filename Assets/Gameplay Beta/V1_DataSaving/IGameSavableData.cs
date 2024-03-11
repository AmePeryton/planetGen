public interface IGameSavableData
{
	void LoadGameSaveData(V1_GameSaveData data);
	void SaveGameSaveData(ref V1_GameSaveData data);
}