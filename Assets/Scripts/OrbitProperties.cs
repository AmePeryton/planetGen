using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitProperties : MonoBehaviour
{
	public float orbitalPeriod; // in REAL YEARS
	public float orbitalDistance;
	public Vector3 orbitalInclination;
	public Vector3 planetPoint;
	public GameObject planetPrefab;
	public GameObject planet;
	public float planetMass;
	public StarProperties starScript;
	public StellarSystem systemScript;

	private float orbitSpeed;
	private float yearToSec = 3.154f * Mathf.Pow(10f, 7f);

	private void Start()
	{
		starScript = systemScript.starList[0].GetComponent<StarProperties>();
		NewOrbitProperties();

		NewPlanet();
	}

	void FixedUpdate()
	{
		CalculateOrbitProperties();
		UpdateOrbit();
	}

	public void NewOrbitProperties()
	{
		planetMass = Random.Range(0.1f, 1000f);
		orbitalDistance = (Random.Range(0.1f * starScript.mass, 50f * starScript.mass));
		orbitalPeriod = Random.Range(0.1f, 20f);
		orbitalInclination = new Vector3(Random.Range(-0.1f, 0.1f), 1f, Random.Range(-0.1f, 0.1f));
	}

	private void CalculateOrbitProperties()
	{

	}

	void UpdateOrbit()
	{
		orbitSpeed = 6f / orbitalPeriod * systemScript.timeScale;
		transform.Rotate(orbitalInclination, orbitSpeed, Space.World);
	}

	public void DestroyOrbit()
	{
		planet.GetComponent<PlanetProperties>().DestroyPlanet();
		Destroy(this.gameObject);
	}

	public void NewPlanet()
	{
		planet = Instantiate(planetPrefab, transform);
		planet.GetComponent<PlanetProperties>().systemScript = systemScript;
		planet.GetComponent<PlanetProperties>().starScript = starScript;
	}

	public void DestroyPlanet()
	{
		planet.GetComponent<PlanetProperties>().DestroyPlanet();
	}
}
