using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLineVector2 : StatLine
{
	public Vector2 value;

	[Header("Inputs and Outputs")]
	public TMP_InputField inputX;
	public TMP_InputField inputY;

	public override void InitDisplay()
	{
		base.InitDisplay();

		UpdateDisplay();

		inputX.onEndEdit.AddListener((i) => {
			value = new Vector2(float.Parse(i), value.y);
			field.SetValue(focus, value);
			UpdateDisplay();
		});
		inputY.onEndEdit.AddListener((i) => {
			value = new Vector2(value.x, float.Parse(i));
			field.SetValue(focus, value);
			UpdateDisplay();
		});
	}

	public override void UpdateDisplay()
	{
		value = (Vector2)field.GetValue(focus);
		if (!inputX.isFocused) { inputX.text = value.x.ToString(); }
		if (!inputY.isFocused) { inputY.text = value.y.ToString(); }
		//inputX.text = value.x.ToString();
		//inputY.text = value.y.ToString();
	}

	public override void SetEditability(bool editable)
	{
		inputX.interactable = editable;
		inputY.interactable = editable;
	}
}