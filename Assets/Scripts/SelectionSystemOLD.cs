using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSystemOLD : MonoBehaviour
{
	public GameObject selected;

	void Start()
	{
		selected = null;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			selected.GetComponent<SelectableObjectOLD>().display.SetActive(false);
			selected = null;
		}
		//Camera.main.GetComponent<CameraController>().target = selected;
	}
}
