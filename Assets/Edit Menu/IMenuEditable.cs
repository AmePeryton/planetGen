using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuEditable
{
	public string GetName();
	public string GetInfo();

	// Classes implementing this should also have at least one field with the MenuEditable attribute
}