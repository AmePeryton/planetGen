using System;
using UnityEngine;

public class V1_StellarCamera : V1_ThirdPersonCameraController
{
	[Header("View Axis OLD")]
	[Obsolete] [NonSerialized]
	public Vector3 viewAxisDefaultOLD;	// Default direction of viewpoint offset from camera
	[Obsolete] [NonSerialized]
	public Vector3 viewAxisTargetOLD;	// Target direction of viewpoint offset from camera
	[Obsolete] [NonSerialized]
	public Vector3 viewAxisCurrentOLD;	// Current direction of viewpoint offset from camera

	private void Awake()
	{
		CameraInit();
		zoomCurrent = 0;	// Cinematic zoom out on load
	}

	private void Update()
	{
		StandardUpdate();
	}

	[Obsolete]
	private void RotateOLD()
	{
		if (Input.GetKey(KeyCode.Mouse1))
		{
			// Rotate horizontally
			viewAxisTargetOLD = Quaternion.AngleAxis(10 * Input.GetAxis("Mouse X"), Vector3.up) * viewAxisTargetOLD;
			// Rotate vertically
			viewAxisTargetOLD = Quaternion.AngleAxis(6 * Input.GetAxis("Mouse Y"), Vector3.Cross(viewAxisTargetOLD, Vector3.up)) * viewAxisTargetOLD;
			// Prevent rotation from being straight up or down (gimbal lock)
			viewAxisTargetOLD.y = Mathf.Clamp(viewAxisTargetOLD.y, -0.95f, 0.95f);
			// Normalize view axis
			viewAxisTargetOLD.Normalize();
		}

		viewAxisCurrentOLD.x = Vector3.Slerp(viewAxisTargetOLD, viewAxisCurrentOLD, 0.98f).normalized.x;
		viewAxisCurrentOLD.y = Mathf.Lerp(viewAxisTargetOLD.y, viewAxisCurrentOLD.y, 0.98f);
		viewAxisCurrentOLD.z = Vector3.Slerp(viewAxisTargetOLD, viewAxisCurrentOLD, 0.98f).normalized.z;
	}
}