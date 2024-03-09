using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectOLD : MonoBehaviour
{
	public SelectionSystemOLD system;
	public GameObject display;
	public Vector3 viewPoint;
	public Vector2 viewRange;

	void Start()
	{
		system = GameObject.Find("Stellar System").GetComponent<SelectionSystemOLD>();
		display.SetActive(false);
	}

	void Update()
	{
		
	}

	private void OnMouseDown()
	{
		if (system.selected != null && system.selected != gameObject)
		{
			system.selected.GetComponent<SelectableObjectOLD>().display.SetActive(false);
		}
		system.selected = gameObject;
		display.SetActive(!display.activeSelf);
	}
}