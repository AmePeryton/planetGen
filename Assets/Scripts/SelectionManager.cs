using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
	public Selectable focus;

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	public void Select(Selectable target)
	{
		if(focus != null)
		{
			Deselect(focus);
		}
		target.selected = true;
		focus = target;
	}

	public void Deselect(Selectable target)
	{
		target.selected = false;
		focus = null;
	}
}