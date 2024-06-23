using UnityEngine;

public class V1_SceneController : MonoBehaviour
{
	public static V1_SceneController instance { get; private set; }

	public GameObject saveDataManagerPrefab;

	protected void SceneControllerInit()
	{
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;

		// If no save data manager present, spawn one
		if (V1_SaveDataManager.instance == null)
		{
			Debug.LogWarning("V1_SaveDataManager not found! Creating new instance");
			Instantiate(saveDataManagerPrefab);
		}
	}

	public virtual void LoadScene() { }	// Load data from the SDM to objects in the scene
	public virtual void SaveScene() { }	// Save data from scene objects to the SDM
	public virtual void NewScene() { }	// On entering the scene from another scene
}