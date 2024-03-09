using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarSystem : MonoBehaviour
{
	public GUIController GUIScript;
	public GameObject myCamera;
	public GameObject starPrefab;
	public GameObject orbitPrefab;
	public List<GameObject> starList;
	public List<GameObject> orbitList;

	[Header("Settings")]
	public float starScale;		// visual scale of stars
	public float planetScale;	// visual scale of planets
	public float timeScale;		// time speed
	public bool viewScale;		// enable / disable viewing scale
	public int minOrbits;		// minimum number of planets
	public int maxOrbits;       // maximum number of planets

	void Start()
	{
		NewSystem();
	}

	public void NewSystem()
	{
		DestroySystem();

		NewStars(1);
		NewOrbits(Random.Range(minOrbits, maxOrbits));
	}

	public void DestroySystem()
	{
		DestroyStars();
		DestroyOrbits();
		GUIScript.GUIReset();
	}

	public void NewStars(int numStars)	// Creates n stars
	{
		int i = 0;
		while (i < numStars)
		{
			GameObject newStar = Instantiate(starPrefab, transform);
			newStar.GetComponent<StarProperties>().systemScript = this;
			starList.Add(newStar);
			i++;
		}
	}

	public void NewOrbits(int numOrbits)	// Creates n orbits
	{
		int i = 0;  // unity no liek for loops
		while (i < numOrbits)
		{
			GameObject newOrbit = Instantiate(orbitPrefab, transform);
			newOrbit.GetComponent<OrbitProperties>().systemScript = this;
			orbitList.Add(newOrbit);
			i++;
		}
	}

	public void DestroyStars()	// Destroys all stars
	{
		foreach (GameObject star in starList)
		{
			star.GetComponent<StarProperties>().DestroyStar();
		}
		starList.Clear();
	}

	public void DestroyOrbits()	// Destroy all orbits
	{
		foreach (GameObject orbit in orbitList)
		{
			orbit.GetComponent<OrbitProperties>().DestroyOrbit();
		}
		orbitList.Clear();
	}
}
