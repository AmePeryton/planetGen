using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLineColor : StatLine
{
	public Color value;

	[Header("Inputs and Outputs")]
	public TMP_InputField inputR;
	public TMP_InputField inputG;
	public TMP_InputField inputB;
	public RawImage swatch;

	public override void InitDisplay()
	{
		base.InitDisplay();

		UpdateDisplay();

		inputR.onEndEdit.AddListener((s) => {
			value = new Color(
				Mathf.Clamp(int.Parse(s)/255f, 0, 1),
				value.g,
				value.b);
			field.SetValue(focus, value);
			UpdateDisplay();
		});
		inputG.onEndEdit.AddListener((s) => {
			value = new Color(
				value.r,
				Mathf.Clamp(int.Parse(s)/255f, 0, 1),
				value.b);
			field.SetValue(focus, value);
			UpdateDisplay();
		});
		inputB.onEndEdit.AddListener((s) => {
			value = new Color(
				value.r,
				value.g,
				Mathf.Clamp(int.Parse(s)/255f, 0, 1));
			field.SetValue(focus, value);
			UpdateDisplay();
		});
	}

	public override void UpdateDisplay()
	{
		value = (Color)field.GetValue(focus);
		if (!inputR.isFocused) { inputR.text = (value.r * 255).ToString(); }
		if (!inputG.isFocused) { inputG.text = (value.g * 255).ToString(); }
		if (!inputB.isFocused) { inputB.text = (value.b * 255).ToString(); }
		//inputR.text = (value.r * 255).ToString();
		//inputG.text = (value.g * 255).ToString();
		//inputB.text = (value.b * 255).ToString();
		swatch.color = value;
	}

	public override void SetEditability(bool editable)
	{
		inputR.interactable = editable;
		inputG.interactable = editable;
		inputB.interactable = editable;
	}
}