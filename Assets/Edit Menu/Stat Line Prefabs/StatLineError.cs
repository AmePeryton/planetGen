using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLineError : StatLine
{
	public TextMeshProUGUI typeLabel;
	public override void InitDisplay()
	{
		base.InitDisplay();

		typeLabel.text = "Unsupported type: " + field.FieldType.ToString();

		UpdateDisplay();
	}

	public override void UpdateDisplay(){}

	public override void SetEditability(bool editable){}
}