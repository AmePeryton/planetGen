using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetPanel : MonoBehaviour
{
	public TextMeshProUGUI massLabel;
	public TextMeshProUGUI radiusLabel;
	public TextMeshProUGUI temperatureLabel;
	public TextMeshProUGUI dayLengthLabel;

	public PlanetProperties planetScript;

	void Start()
	{
	}

	void Update()
	{
		massLabel.text = planetScript.mass + " M";
		radiusLabel.text = planetScript.radius + " R";
		temperatureLabel.text = planetScript.temperature + " kK";
		dayLengthLabel.text = planetScript.dayLength + " s";
	}

	public void RemakeOrbit()
	{
	}

	public void RemakePlanet()
	{
		// TODO: edit scripts so that pressing the remake planet button destroys the planet
		//	and prompts the parent orbit to create a new planet
		planetScript.NewPlanetProperties();
	}
}