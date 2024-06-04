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
		labelType.text = focus.GetType().ToString();
		labelName.text = focus.GetName();
	}

	public void SelectFocus()
	{
		EditMenu.instance.NewFocus(focus);
		GetComponent<Button>().interactable = false;
	}
}