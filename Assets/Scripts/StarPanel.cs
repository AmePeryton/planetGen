using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarPanel : MonoBehaviour
{
	public TextMeshProUGUI classLabel;
	public TextMeshProUGUI massLabel;
	public TextMeshProUGUI radiusLabel;
	public TextMeshProUGUI temperatureLabel;
	public TextMeshProUGUI luminosityLabel;

	public StarProperties starScript;

	void Start()
	{
	}

	void Update()
	{
		classLabel.text = "Class " + starScript.stellarClass + " Star";
		massLabel.text = starScript.mass + " M";
		radiusLabel.text = starScript.radius + " R";
		temperatureLabel.text = starScript.temperature + " kK";
		luminosityLabel.text = starScript.luminosity + " L";
	}
}
