using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLineVector3 : StatLine
{
	public Vector3 value;

	[Header("Inputs and Outputs")]
	public TMP_InputField inputX;
	public TMP_InputField inputY;
	public TMP_InputField inputZ;

	public override void InitDisplay()
	{
		base.InitDisplay();

		UpdateDisplay();

		inputX.onEndEdit.AddListener((s) => {
			value = new Vector3(float.Parse(s), value.y, value.z);
			field.SetValue(focus, value);
			UpdateDisplay();
		});
		inputY.onEndEdit.AddListener((s) => {
			value = new Vector3(value.x, float.Parse(s), value.z);
			field.SetValue(focus, value);
			UpdateDisplay();
		});
		inputZ.onEndEdit.AddListener((s) => {
			value = new Vector3(value.x, value.y, float.Parse(s));
			field.SetValue(focus, value);
			UpdateDisplay();
		});
	}

	public override void UpdateDisplay()
	{
		value = (Vector3)field.GetValue(focus);
		if (!inputX.isFocused) { inputX.text = value.x.ToString(); }
		if (!inputY.isFocused) { inputY.text = value.y.ToString(); }
		if (!inputZ.isFocused) { inputZ.text = value.z.ToString(); }
		//inputX.text = value.x.ToString();
		//inputY.text = value.y.ToString();
		//inputZ.text = value.z.ToString();
	}

	public override void SetEditability(bool editable)
	{
		inputX.interactable = editable;
		inputY.interactable = editable;
		inputZ.interactable = editable;
	}
}