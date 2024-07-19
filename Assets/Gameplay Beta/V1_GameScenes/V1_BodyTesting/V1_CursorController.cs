using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_CursorController : MonoBehaviour
{
	public Camera cam;
	public Vector3 worldPosition;
	public V1_BodyPartController selectedPart;
	public float apparentSize;
	public float trueSize;

	void Start()
	{
		selectedPart = null;
	}

	void Update()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hitData))
		{
			worldPosition = hitData.point;

			if (Input.GetKey(KeyCode.Mouse0))
			{
				if (hitData.collider.GetComponent<V1_BodyPartController>() != null)
				{
					Debug.Log(hitData.collider.gameObject.name);
				}
			}
		}
		else
		{
			worldPosition = Vector3.zero;
			trueSize = 0;
		}

		trueSize = apparentSize * hitData.distance;
		transform.localScale = trueSize * Vector3.one;
		transform.position = worldPosition;
	}
}