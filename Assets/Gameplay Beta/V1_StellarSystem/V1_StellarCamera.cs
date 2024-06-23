using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_StellarCamera : MonoBehaviour
{
	[Header("View Point")]
	public Vector3 viewPointDefault;	// Default position to point at
	public Vector3 viewPointTarget;		// Default position to point at
	public Vector3 viewPointCurrent;	// Current position pointed at

	[Header("View Axis")]
	public Vector3 viewAxisDefault;	// Default direction of viewpoint offset from camera
	public Vector3 viewAxisTarget;	// Target direction of viewpoint offset from camera
	public Vector3 viewAxisCurrent; // Current direction of viewpoint offset from camera

	[Header("Offset Angle")]
	public Vector3 offsetAngleDefault;
	public Vector3 offsetAngleTarget;
	public Vector3 offsetAngleCurrent;

	[Header("Zoom")]
	public float[] zoomSettings = new float[5];	// closest, farthest, default (real), step (percent), smoothing
	public float zoomPercent;	// Target percentage of zoom in/out (0 to 1)
	public float zoomTarget;	// Target distance calculated from the percentage
	public float zoomCurrent;	// Current distance that is being lerped to the target distance

	private void Awake()
	{
		viewAxisTarget = viewAxisDefault;
		viewAxisCurrent = viewAxisTarget;

		viewPointTarget= viewPointDefault;
		viewPointCurrent = viewPointTarget;

		zoomCurrent = zoomTarget;

		// Convert default distance to percent
		// Cuberoot( (default - min) / (max - min) )
		zoomPercent = Mathf.Pow((zoomSettings[2] - zoomSettings[0]) / (zoomSettings[1] - zoomSettings[0]), 1f / 4f);

		transform.position = viewPointCurrent - zoomCurrent * viewAxisTarget;
	}

	void Update()
	{
		Move();
		Rotate();
		Zoom();

		// Lerp current zoom to target zoom
		transform.position = viewPointCurrent - zoomCurrent * viewAxisCurrent;

		// Look at current view point
		transform.LookAt(viewPointCurrent);
	}

	private void Move()
	{
		viewPointCurrent = Vector3.Lerp(viewPointTarget, viewPointCurrent, 0.99f);
	}

	private void Rotate()
	{
		if (Input.GetKey(KeyCode.Mouse2))
		{
			// Rotate horizontally
			viewAxisTarget = Quaternion.AngleAxis(10 * Input.GetAxis("Mouse X"), Vector3.up) * viewAxisTarget;
			// Rotate vertically
			viewAxisTarget = Quaternion.AngleAxis(6 * Input.GetAxis("Mouse Y"), Vector3.Cross(viewAxisTarget, Vector3.up)) * viewAxisTarget;
			// Prevent rotation from being straight up ordown (gimbal lock)
			viewAxisTarget.y = Mathf.Clamp(viewAxisTarget.y, -0.95f, 0.95f);
			// Normalize view axis
			viewAxisTarget.Normalize();
		}

		viewAxisCurrent.x = Vector3.Slerp(viewAxisTarget, viewAxisCurrent, 0.98f).normalized.x;
		viewAxisCurrent.y = Mathf.Lerp(viewAxisTarget.y, viewAxisCurrent.y, 0.98f);
		viewAxisCurrent.z = Vector3.Slerp(viewAxisTarget, viewAxisCurrent, 0.98f).normalized.z;
	}

	// Update zooming variables based on scroll
	private void Zoom()
	{
		// Input mouse scroll and clamp 0-1
		zoomPercent -= Input.mouseScrollDelta.y * zoomSettings[3];
		zoomPercent = Mathf.Clamp01(zoomPercent);

		// Calculate real zoom distance based on zoom percentage and zoom settings
		zoomTarget = ((zoomSettings[1] - zoomSettings[0]) * Mathf.Pow(zoomPercent, 4) + zoomSettings[0]);

		// Lerp current distance to target distance
		zoomCurrent = Mathf.Lerp(zoomTarget, zoomCurrent, zoomSettings[4]);
	}

}

// TODO:
/*	minor lerp view axis
 *	major lerp view distance (zoom)
 *	minor lerp current view point
 */