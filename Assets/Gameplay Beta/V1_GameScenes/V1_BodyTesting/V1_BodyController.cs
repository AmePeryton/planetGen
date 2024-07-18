using UnityEngine;

public class V1_BodyController : MonoBehaviour
{
	public V1_BodyData data;
	public V1_BodyPartController coreController;
	public V1_BodyPartController selectedPart;
	public static int partCounter;

	[Header("Prefabs")]
	public GameObject bodyPartPrefab;

	void Start()
	{
		data = new V1_BodyData();

		partCounter = 0;

		NewBody();
	}

	[ContextMenu("New Body")]
	public void NewBody()
	{
		// Delete old core
		if (coreController != null)
		{
			// Recursively destroy all body parts and joints
			coreController.DestroyBodyPart();
		}

		// Spawn new core
		coreController = Instantiate(bodyPartPrefab).GetComponent<V1_BodyPartController>();
		//coreController.Awake();	// Awake seems to run as intended here anyway
		coreController.BodyPartInit(new V1_BodyPartData());
		data.core = coreController.data;
		selectedPart = coreController;
	}

	[ContextMenu("Add Part to Selected")]
	public void AddPartToSelected()
	{
		selectedPart.CreateNewBodyPart();
	}

	[ContextMenu("Load Data")]
	public void Load()
	{
		// Delete old core
		if (coreController != null)
		{
			// Recursively destroy all body parts and joints
			coreController.DestroyBodyPart();
		}

		// TODO: get references to relevant data from save data manager
		data = V1_FileHandler.Load<V1_BodyData>(Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + "BODY TESTING" + ".save");

		// Spawn core as defined in the save data
		coreController = Instantiate(bodyPartPrefab).GetComponent<V1_BodyPartController>();
		//coreController.Awake();	// Awake seems to run as intended here anyway
		coreController.BodyPartInit(data.core);
		selectedPart = coreController;
	}

	[ContextMenu("Save Data")]
	public void Save()
	{
		V1_FileHandler.Save(data, Application.dataPath + "/Gameplay Beta/V1_GameFiles/" + "BODY TESTING" + ".save");
	}
}

[SerializeField]
public class V1_BodyData
{
	public V1_BodyPartData core;	// The root of the body part tree, connected by joint

	public V1_BodyData()
	{
		core = null;
	}
}