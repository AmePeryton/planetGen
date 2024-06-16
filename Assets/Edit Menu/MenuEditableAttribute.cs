using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class MenuEditableAttribute : PropertyAttribute
{
	public string displayName;
	public bool editable;
	public float min;
	public float max;

	public MenuEditableAttribute(string displayName)
	{
		this.displayName = displayName;
		this.editable = true;
		this.min = 0;
		this.max = 1;
	}

	public MenuEditableAttribute(string displayName, bool editable)
	{
		this.displayName = displayName;
		this.editable = editable;
		this.min = 0;
		this.max = 1;
	}

	public MenuEditableAttribute(string displayName, float min, float max)
	{
		this.displayName = displayName;
		this.editable = true;
		this.min = min;
		this.max = max;
	}

	public MenuEditableAttribute(string displayName, bool editable, float min, float max)
	{
		this.displayName = displayName;
		this.editable = editable;
		this.min = min;
		this.max = max;
	}
}