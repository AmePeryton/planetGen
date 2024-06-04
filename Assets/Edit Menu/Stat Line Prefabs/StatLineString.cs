using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLineString : StatLine
{
	public string value;

	[Header("Inputs and Outputs")]
	public TMP_InputField input;

	public override void InitDisplay()
	{
		base.InitDisplay();

		UpdateDisplay();
		
		input.onEndEdit.AddListener((s) => {
			value = s;
			field.SetValue(focus, value);
			UpdateDisplay();
		});
	}

	public override void UpdateDisplay()
	{
		value = (string)field.GetValue(focus);
		if (!input.isFocused) { input.text = value; }
	}

	public override void SetEditability(bool editable)
	{
		input.interactable = editable;
	}
}