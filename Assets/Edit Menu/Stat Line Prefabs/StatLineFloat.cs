using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLineFloat : StatLine
{
	public float value;

	[Header("Inputs and Outputs")]
	public Slider slider;
	public TMP_InputField input;

	public override void InitDisplay()
	{
		base.InitDisplay();

		slider.minValue = attribute.min;
		slider.maxValue = attribute.max;
		UpdateDisplay();

		slider.onValueChanged.AddListener((f) => {
			value = Mathf.Clamp(f, attribute.min, attribute.max);
			field.SetValue(focus, value);
			UpdateDisplay();
		});
		input.onEndEdit.AddListener((s) => {
			value = Mathf.Clamp(float.Parse(s), attribute.min, attribute.max);
			field.SetValue(focus, value);
			UpdateDisplay();
		});
	}

	public override void UpdateDisplay()
	{
		value = (float)field.GetValue(focus);
		slider.value = value;
		if (!input.isFocused) { input.text = value.ToString(); }
	}

	public override void SetEditability(bool editable)
	{
		slider.interactable = editable;
		input.interactable = editable;
	}
}