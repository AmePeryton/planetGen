using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FocusLine : MonoBehaviour
{
	public TextMeshProUGUI labelType;
	public TextMeshProUGUI labelName;

	public IMenuEditable focus;

	public void InitDisplay()
	{
		// Set text labels
		labelType.text = focus.GetType().ToString();
		labelName.text = focus.GetName();
	}

	public void SelectFocus()
	{
		// Set this menu editable instance as the current focus
		EditMenu.instance.NewFocus(focus);
	}
}