using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLineBool : StatLine
{
	public bool value;

	[Header("Inputs and Outputs")]
	public Toggle toggle;

	public override void InitDisplay()
	{
		base.InitDisplay();

		UpdateDisplay();

		toggle.onValueChanged.AddListener((b) => {
			value = b;
			field.SetValue(focus, value);
			UpdateDisplay();
		});
	}

	public override void UpdateDisplay()
	{
		value = (bool)field.GetValue(focus);
		toggle.isOn = value;
	}

	public override void SetEditability(bool editable)
	{
		toggle.interactable = editable;
	}
}