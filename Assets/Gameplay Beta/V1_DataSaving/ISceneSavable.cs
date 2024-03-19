public interface ISceneSavable
{
	void LoadSceneData(V1_GameSaveData data);
	void SaveSceneData(ref V1_GameSaveData data);
}