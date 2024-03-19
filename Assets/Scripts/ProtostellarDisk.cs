using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtostellarDisk : MonoBehaviour
{
	public float timeScale;
	public float initialVelocityNoise;
	public float protostarMass; // in kg

	public List<PlanetesimalProperties> planetesimals;
	public int numPlanetesimal;
	public Vector2 massLimits;	// in kg
	public float spawnRange; // in AU

	private int ID;
	private float gravConstant = 6.6743f * Mathf.Pow(10, -11);  // m^3 / (kg * s^2)
	private float earthMassToKG = 5.972f * Mathf.Pow(10, 24);   // kg / E_m
	private float solarMassToKG = 1.989f * Mathf.Pow(10, 30);   // kg / S_m
	private float AUtoM = 1.496f * Mathf.Pow(10, 11);		   // m / AU

	[Header ("Statistics")]
	public float solarMasses;
	public float secondsPassed;
	public float yearsPassed;
	public GameObject mostMassiveBodyObject;
	public float mostMassiveBodyMass;
	public float totalMass;

	[Header ("Inspector Controls")]
	public bool visualEffects;
	public float visualScale;
	public float visualScaleStar;
	public bool setSeed;	// enable before running to manually set the seed, disable for a random seed
	public int currentSeed; // displays the current seed
	public bool resetDisk;
	public bool debugEffects;	private bool prevDebugEffects;

	[Header("Presets")] // values that must be set in the prefab inspector before running
	public GameObject visualSphere;
	public GameObject planetesimalPrefab;
	public GravitySimulator gravityScript;

	void Start()
	{
		newProperties();
	}

	void Update()
	{
		if(resetDisk)
		{
			resetDisk = false;
			newProperties();
		}

		planetesimals.RemoveAll(i => i == null);
		mostMassiveBodyMass = 0;
		totalMass = 0;
		solarMasses = protostarMass / solarMassToKG;

		foreach (PlanetesimalProperties planetesimal in planetesimals)
		{
			planetesimal.timeScale = timeScale;
			planetesimal.visualScale = visualScale;
			planetesimal.visualEffects = visualEffects;

			if(prevDebugEffects != debugEffects)
			{
				planetesimal.debugEffects = debugEffects;
			}

			totalMass += planetesimal.mass;
			if (planetesimal.mass >= mostMassiveBodyMass)
			{
				mostMassiveBodyObject = planetesimal.gameObject;
				mostMassiveBodyMass = planetesimal.mass;
			}
		}
		TrackTime();
		VisualUpdate();
		prevDebugEffects = debugEffects;
	}

	void SetStartingVelocity(PlanetesimalProperties planetesimalScript, float noiseScale)
	{
		Vector3 output = Vector3.Cross(Vector3.up, planetesimalScript.transform.position - transform.position).normalized;
		output = output * Mathf.Sqrt((gravConstant * (planetesimalScript.gravityScript.mass + gravityScript.mass)) /
			(Vector3.Distance(planetesimalScript.transform.position, transform.position) * AUtoM));
		output = output * Random.Range(1 - noiseScale, 1 + noiseScale);
		planetesimalScript.gravityScript.velocityVector = output;
	}

	// Shamelessly stolen from https://answers.unity.com/questions/421968/normal-distribution-random.html
	public static float RandomGaussian(float minValue, float maxValue)
	{
		float u, v, S;

		do 
		{
			u = 2.0f * UnityEngine.Random.value - 1.0f;
			v = 2.0f * UnityEngine.Random.value - 1.0f;
			S = u * u + v * v;
		}
		while(S >= 1.0f);

		// Standard Normal Distribution
		float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

		// Normal Distribution centered between the min and max value
		// and clamped following the "three-sigma rule"
		float mean = (minValue + maxValue) / 2.0f;
		float sigma = (maxValue - mean) / 3.0f;
		return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
	}

	public void TrackTime()
	{
		secondsPassed += Time.deltaTime * timeScale;
		//if (secondsPassed > 31556926)
		//{
		//	secondsPassed = secondsPassed - 31556926;
		//	yearsPassed += 1;
		//}

		yearsPassed += (int) secondsPassed / 31556926;
		secondsPassed = secondsPassed % 31556926;
	}

	public void VisualUpdate()
	{
		visualSphere.transform.localScale = 0.00465047f * Vector3.one * 2 * visualScaleStar;
	}

	public void newProperties()
	{
		prevDebugEffects = !debugEffects;
		ID = 0;
		foreach (PlanetesimalProperties planetesimal in planetesimals)
		{
			planetesimal.SelfDestruct(false, "Remaking Protostellar Disk");
		}

		if (setSeed)
		{
			Random.InitState(currentSeed);
		}
#pragma warning disable CS0618
		currentSeed = Random.seed;
#pragma warning restore CS0618

		gravityScript.mass = protostarMass;
		for (int i = 0; i < numPlanetesimal; i++)
		{
			GameObject newPlanetesimal = Instantiate(planetesimalPrefab, transform);
			PlanetesimalProperties newPlanetesimalProperties = newPlanetesimal.GetComponent<PlanetesimalProperties>();
			newPlanetesimalProperties.mass = RandomFromDistribution.RandomRangeExponential(massLimits.x, massLimits.y, 100, RandomFromDistribution.Direction_e.Left);
			newPlanetesimalProperties.timeScale = timeScale;
			Vector2 randomPosition = new Vector2(RandomGaussian(-spawnRange, spawnRange), RandomGaussian(-spawnRange, spawnRange));
			float inclineOffset = randomPosition.magnitude * Mathf.Tan(RandomGaussian(-0.2f, 0.2f));
			newPlanetesimal.transform.position = new Vector3(randomPosition.x, inclineOffset, randomPosition.y);
			newPlanetesimal.name = "Planetesimal #" + (ID++).ToString();
			SetStartingVelocity(newPlanetesimalProperties, initialVelocityNoise);
			planetesimals.Add(newPlanetesimalProperties);
		}

	}
}