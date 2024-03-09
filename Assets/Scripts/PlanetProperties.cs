using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetProperties : MonoBehaviour
{
	public float mass;
	public float radius;
	public float dayLength;		// in in-game seconds
	public float temperature;
	public Color color;
	public StellarSystem systemScript;
	public StarProperties starScript;
	public float gameScale;
	public List<StarProperties> systemStars;

	private float rotationSpeed;
	private Material mymat;
	public OrbitProperties orbitScript;
	//private SelectableObject selectionScript;
	private TrailRenderer trail;
	private float earthToAU = 0.0000426352f;

	void Start()
	{
		systemStars = new List<StarProperties>();
		orbitScript = transform.parent.GetComponent<OrbitProperties>();

		NewPlanetProperties();

		//selectionScript = transform.GetComponent<SelectableObject>();
		mymat = GetComponent<Renderer>().material;
		trail = GetComponent<TrailRenderer>();
	}

	void FixedUpdate()
	{

		CalculatePlanetProperties();
		UpdatePlanet();

		Rotate();
	}

	public void NewPlanetProperties()
	{
		trail = GetComponent<TrailRenderer>();
		trail.enabled = false;
		mass = orbitScript.planetMass;
		radius = mass;
		dayLength = Random.Range(0.5f, 10f);
		//color = Random.ColorHSV();
		color = Color.green;
	}

	private void CalculatePlanetProperties()
	{

	}

	private void UpdatePlanet()	// maintain visual components (size, color, etc.) to match internal variables
	{
		trail.enabled = true;
		mymat.color = color;
		mymat.SetColor("_EmissionColor", color);
		trail.startColor = color;
		trail.endColor = new Color(color.r, color.g, color.b, 0);
		transform.localPosition = new Vector3(orbitScript.orbitalDistance, 0f, 0f);

		gameScale = radius * earthToAU;
		if (systemScript.viewScale)
		{
			gameScale = radius * earthToAU * systemScript.planetScale;
		}
		transform.localScale = new Vector3(gameScale, gameScale, gameScale);

		//selectionScript.viewPoint = transform.position + new Vector3(0, 5, 0);
		//selectionScript.viewRange = new Vector2(0, 10);
	}

	private void Rotate()
	{
		rotationSpeed = 6f / dayLength;
		transform.Rotate(0f, rotationSpeed, 0f);
	}

	public void DestroyPlanet()
	{
		Destroy(this.gameObject);
	}

	private void OnMouseDown()
	{
		systemScript.GUIScript.TogglePlanetPanel(gameObject);
	}
}
