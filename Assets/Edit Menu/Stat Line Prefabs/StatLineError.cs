using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLineError : StatLine
{
	public override void InitDisplay()
	{
		base.InitDisplay();

		UpdateDisplay();
	}

	public override void UpdateDisplay(){}

	public override void SetEditability(bool editable){}
}