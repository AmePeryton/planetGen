using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarProperties : MonoBehaviour
{
	public ClassName stellarClass;
	public GameObject viewPoint;
	public float mass;              // in Solar Mass		// in kg
	public float radius;            // in Solar Radius		// in km
	public float temperature;		// in kiloKelvin		// in Kelvin
	public float luminosity;		// in Solar Luminosity
	public Color mainColor;			// inner cell color
	public Color secondaryColor;	// cell edge color
	public Material myMaterial;
	public enum ClassName { X, M, K, G, F, A, B, O }
	public StellarSystem systemScript;
	public float gameScale;

	private float[] starTypeLimits = { 2.4f, 3.7f, 5.2f, 6.0f, 7.5f, 10.0f, 30.0f, 50.0f };
	private StellarClass[] mainSequence = {
		new StellarClass(ClassName.X, Color.HSVToRGB(0f, 0f, 0f)),
		new StellarClass(ClassName.M, Color.HSVToRGB(0.01f, 1f, 0.7f)),
		new StellarClass(ClassName.K, Color.HSVToRGB(0.06f, 0.9f, 0.8f)),
		new StellarClass(ClassName.G, Color.HSVToRGB(0.14f, 0.8f, 0.7f)),
		new StellarClass(ClassName.F, Color.HSVToRGB(0.15f, 0.4f, 0.7f)),
		new StellarClass(ClassName.A, Color.HSVToRGB(0.6f, 0.1f, 0.6f)),
		new StellarClass(ClassName.B, Color.HSVToRGB(0.64f, 0.8f, 0.8f)),
		new StellarClass(ClassName.O, Color.HSVToRGB(0.67f, 0.95f, 1f))
	};
	private StellarClass stellarClassType;
	//private SelectableObject selectionScript;

	private float solarRadiusToAU = 0.00465047f;				// AU / S_r
	private float solarMassToKG = 1.989f * Mathf.Pow(10, 30);   // kg / S_m
	private float AUtoM = 1.496f * Mathf.Pow(10, 11);           // m / AU

	void Start()
	{
		NewStarProperties();
		//selectionScript = transform.GetComponent<SelectableObject>();
	}

	void FixedUpdate()
	{
		CalculateStarProperties();
		UpdateStar();
	}

	public void NewStarProperties()
	{
		mass = PiecewiseMass(Random.value);
	}

	private void CalculateStarProperties()
	{
		luminosity = Mathf.Pow(mass, 4f);
		radius = Mathf.Pow(mass, 0.74f);
		temperature = 5.778f * Mathf.Pow(mass, 0.505f);

		if (temperature < starTypeLimits[0])
		{	//	Unknown
			stellarClassType = mainSequence[0];
		}
		else if (temperature < starTypeLimits[1])
		{	//	M
			stellarClassType = mainSequence[1];
		}
		else if (temperature < starTypeLimits[2])
		{	//	K
			stellarClassType = mainSequence[2];
		}
		else if (temperature < starTypeLimits[3])
		{	//	G
			stellarClassType = mainSequence[3];
		}
		else if (temperature < starTypeLimits[4])
		{	//	F
			stellarClassType = mainSequence[4];
		}
		else if (temperature < starTypeLimits[5])
		{	//	A
			stellarClassType = mainSequence[5];
		}
		else if (temperature < starTypeLimits[6])
		{	//	B
			stellarClassType = mainSequence[6];
		}
		else if (temperature < starTypeLimits[7])
		{	//	O
			stellarClassType = mainSequence[7];
		}
		else
		{	//	Unknown
			stellarClassType = mainSequence[0];
		}

		stellarClass = stellarClassType.name;
		mainColor = stellarClassType.color;

		viewPoint.transform.position = new Vector3(0f, 10f + radius, 0f);
	}

	private void UpdateStar()
	{
		gameScale = radius * solarRadiusToAU;
		if (systemScript.viewScale) 
		{
			gameScale = radius * solarRadiusToAU * systemScript.starScale;
		}
		transform.localScale = new Vector3(gameScale, gameScale, gameScale);
		myMaterial = GetComponent<Renderer>().material;
		secondaryColor = Color.Lerp(mainColor, Color.black, 0.25f);
		myMaterial.SetColor("_BaseColor", mainColor);
		myMaterial.SetColor("_CellColor", secondaryColor);

		//selectionScript.viewPoint = transform.position + new Vector3(0, 10, 0);
		//selectionScript.viewRange = new Vector2(0, 20);
	}

	private float PiecewiseMass(float massPercentile)
	{
		if (massPercentile < 0.3)
		{
			return 0.8f * massPercentile + 0.18f;
		}
		else if (massPercentile < 0.55)
		{
			return 1.56f * massPercentile - 0.058f;
		}
		else if (massPercentile < 0.7)
		{
			return 1.8f * massPercentile - 0.18f;
		}
		else if (massPercentile < 0.85)
		{
			return 4f * massPercentile - 1.72f;
		}
		else if (massPercentile < 0.93)
		{
			return 16f * massPercentile - 11.92f;
		}
		else if (massPercentile < 0.98)
		{
			return 462.8f * massPercentile - 427.444f;
		}
		else
		{
			return 2285f * massPercentile - 2213.2f;
		}
	}

	public struct StellarClass
	{
		public ClassName name;
		public Color color;

		public StellarClass(ClassName name, Color color)
		{
			this.name = name;
			this.color = color;
		}
	}

	public void DestroyStar()
	{
		Destroy(this.gameObject);
	}

	private void OnMouseDown()
	{
		systemScript.GUIScript.ToggleStarPanel(gameObject);
	}
}
