using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TissueTester : MonoBehaviour
{
	public List<TissueType> tissueTypes;
	public List<TissueOrb> tissueOrbs;
	public List<CavityCylinder> cavityCylinders;
	public List<EnvironmentCube> environmentCubes;

	[Header("Prefabs")]
	public GameObject tissueOrbPrefab;
	public GameObject cavityCylinderPrefab;
	public GameObject environmentCubePrefab;
	public GameObject neighborLinePrefab;

	[Header("Test Variables")]
	public GameObject[] connectables;
	public int selectedTissueType;

	private void Awake()
	{
		tissueOrbs = new List<TissueOrb>();
		cavityCylinders = new List<CavityCylinder>();
		environmentCubes = new List<EnvironmentCube>();
	}

	void Start()
	{
		connectables = new GameObject[2];
	}

	[ContextMenu("New Tissue Type")]
	public void NewTissueType()
	{
		tissueTypes.Add(new TissueType("NEW TISSUE TYPE"));
	}

	public void NewTissue(TissueType t)
	{
		TissueOrb newTissueOrb = Instantiate(tissueOrbPrefab).GetComponent<TissueOrb>();
		newTissueOrb.tissue = new Tissue("NEW TISSUE", t);
		tissueOrbs.Add(newTissueOrb);
	}

	[ContextMenu("New Tissue")]
	public void NewTissue_MenuFriendly()
	{
		if (selectedTissueType < tissueTypes.Count)
		{
			NewTissue(tissueTypes[selectedTissueType]);
		}
		else
		{
			Debug.Log("No tissue type selected");	
		}
	}

	[ContextMenu("New Cavity")]
	public void NewCavity()
	{
		CavityCylinder newCavityCylinder = Instantiate(cavityCylinderPrefab).GetComponent<CavityCylinder>();
		newCavityCylinder.cavity = new Cavity("NEW CAVITY");
		cavityCylinders.Add(newCavityCylinder);
	}

	[ContextMenu("New Environment")]
	public void NewEnvironment()
	{
		EnvironmentCube newEnvironmentCube = Instantiate(environmentCubePrefab).GetComponent<EnvironmentCube>();
		newEnvironmentCube.environment = new Environment("NEW ENVIRONMENT");
		environmentCubes.Add(newEnvironmentCube);
	}

	[ContextMenu("Connect Tissues")]
	public void ConnectTissues()
	{
		ITissueNeighbor a = connectables[0].GetComponent<ITissueNeighbor>();
		ITissueNeighbor b = connectables[1].GetComponent<ITissueNeighbor>();

		// TODO: fix connections

		if (a != null && b != null && a != b)
		{
			a.AddNeighbor(b);
			b.AddNeighbor(a);

			Instantiate(neighborLinePrefab, connectables[0].transform).GetComponent<NeighborLine>().points =
				new GameObject[] { connectables[0], connectables[1] };
			Instantiate(neighborLinePrefab, connectables[1].transform).GetComponent<NeighborLine>().points =
				new GameObject[] { connectables[1], connectables[0] };
		}
		else
		{
			Debug.Log("nah.");
		}
	}

	[ContextMenu("Update All Displays")]
	public void UpdateAllDisplays()
	{
		foreach (TissueOrb t in tissueOrbs)
		{
			t.UpdateDisplay();
		}
		foreach (CavityCylinder c in cavityCylinders)
		{
			c.UpdateDisplay();
		}
		foreach (EnvironmentCube e in environmentCubes)
		{
			e.UpdateDisplay();
		}
	}
}