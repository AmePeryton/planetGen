using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBox : MonoBehaviour
{
	public TextMeshProUGUI classLabel;
	public TextMeshProUGUI massLabel;
	public TextMeshProUGUI radiusLabel;
	public TextMeshProUGUI temperatureLabel;
	public TextMeshProUGUI luminosityLabel;
	public StarProperties starScript;

	private float oldMass;

	void Start()
	{
		oldMass = -1f;
	}

	void Update()
	{
		oldMass = starScript.mass;
		classLabel.text = "Class " + starScript.stellarClass + " Star";
		massLabel.text = SigFigString(starScript.mass, 3) + " M";
		radiusLabel.text = SigFigString(starScript.radius, 3) + " R";
		temperatureLabel.text = SigFigString(starScript.temperature, 3) + " kK";
		luminosityLabel.text = SigFigString(starScript.luminosity, 3) + " L";
	}

	// automatic conversion to scientific format in .ToString() messes this up at very long numbers
	public string SigFigString(float input, int sigFigs)
	{
		if (input == 0) { return "0"; }
		int logScale = Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(input))) + 1;
		float scale = Mathf.Pow(10f, logScale - sigFigs);
		int desiredChars;
		float significant = Mathf.Round(input / scale) * scale;

		string output = significant.ToString();

		if (logScale >= sigFigs) { desiredChars = logScale; }
		else if (logScale < 1) { desiredChars = Mathf.Abs(logScale - 1) + sigFigs + 1; }
		else { desiredChars = sigFigs + 1; }
		if (input < 0f) { desiredChars += 1; }

		string scientific = ScientificNotation(input, sigFigs);
		if (desiredChars >= scientific.Length)
		{
			return scientific;
		}

		while (output.Length < desiredChars)
		{
			if(output.Length == logScale) { output += "."; }
			output += "0";
		}
		if (output.Length > desiredChars) { output = output.Substring(0, desiredChars); }

		return output;
	}

	public string ScientificNotation(float input, int sigFigs)
	{
		string output = input.ToString("E" + (sigFigs - 1));    // standard scientific
		output = output.Substring(0, (output.Length - 3));
		output += Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(input)));
		return output;
	}
}