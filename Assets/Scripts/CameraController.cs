using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 defaultViewPoint;
	public Vector3 realViewPoint;
	//public GameObject target;
	//public SelectableObject targetScript;
	public float moveSpeed;
	public float[] zoomSettings = new float[5];	 // closest, farthest, default (real), step (percent), smoothing
	public float zoomPercent;
	public float zoomReal;

	void Start()
	{
		// convert default distance to percent
		zoomPercent = Mathf.Pow((zoomSettings[2] - zoomSettings[0]) / (zoomSettings[1] - zoomSettings[0]), 1f/3f);
	}

	void Update()
	{
		Zoom();
		transform.position = Vector3.Lerp(new Vector3(0, zoomReal, 0), transform.position, zoomSettings[4]);
		transform.LookAt(Vector3.zero);
	}

	void Zoom()
	{
		zoomPercent -= Input.mouseScrollDelta.y * zoomSettings[3];
		zoomPercent = Mathf.Clamp(zoomPercent, 0, 1);
		zoomReal = ((zoomSettings[1] - zoomSettings[0]) * Mathf.Pow(zoomPercent, 4) + zoomSettings[0]);
	}
}
