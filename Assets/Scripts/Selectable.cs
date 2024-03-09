using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
	public bool selected;
	public GameObject parent;
	public GameObject display;
	public SelectionManager manager;

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	private void OnMouseDown()
	{
		if (!selected)
		{
			manager.Select(this);
		}
		else
		{
			manager.Deselect(this);
		}
	}
}