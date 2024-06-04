using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatLine : MonoBehaviour
{
	public TextMeshProUGUI label;

	public IMenuEditable focus;
	public FieldInfo field;
	public MenuEditableAttribute attribute;
	public abstract void UpdateDisplay();
	public abstract void SetEditability(bool editable);

	public virtual void InitDisplay()
	{
		// Get attribute info
		attribute = field.GetCustomAttribute<MenuEditableAttribute>();
		// Set name label
		label.text = attribute.displayName;
		// Enable or disable inputs
		SetEditability(attribute.editable);
		// Update display every x seconds
		InvokeRepeating(nameof(UpdateDisplay), 0.1f, 0.1f);
	}

	/* TODO
		* fix statline resizing for different resolutions
		* make focus lines reload on new object instantiation
		* add click selection for focus
	*/
}