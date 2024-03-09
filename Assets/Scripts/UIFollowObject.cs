using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowObject : MonoBehaviour
{
	public GameObject target;
	public Vector3 offset;
	public float smoothing;
	public Vector3 publicPos;

	private Camera cam;

	void Start()
	{
		cam = Camera.main;
	}

	void FixedUpdate()
	{
		publicPos = cam.WorldToScreenPoint(target.transform.position);
		publicPos.z = 0;
		transform.position = publicPos;
	}
}