using System;
using UnityEngine;

public class V1_ThirdPersonCameraController : MonoBehaviour
{
	[Header("Camera Settings")]
	public MoveSettings moveSettings;
	public RotateSettings rotateSettings;
	public ZoomSettings zoomSettings;

	[Header("Focus Point")]
	public Vector3 focusPointTarget;	// Default position to point at
	public Vector3 focusPointCurrent;	// Current position pointed at

	[Header("Offset Angle")]
	public Vector3 offsetAngleTarget;	// Target angle from target to camera
	public Vector3 offsetAngleCurrent;	// Current angle from target to camera
	protected Vector3 trueOffsetAxis;

	[Header("Zoom")]
	public float zoomPercent;	// Target percentage of zoom in/out (0 to 1)
	public float zoomTarget;	// Target distance calculated from the percentage
	public float zoomCurrent;	// Current distance that is being lerped to the target distance

	private void Awake()
	{
		CameraInit();
	}

	private void Update()
	{
		StandardUpdate();
	}

	public void CameraInit()
	{
		focusPointTarget = moveSettings.defaultFocusPoint;
		focusPointCurrent = focusPointTarget;

		offsetAngleTarget = rotateSettings.defaultRotation;
		offsetAngleCurrent = offsetAngleTarget;

		SetZoomDistance(zoomSettings.defaultDistance);
		zoomCurrent = zoomTarget;

		transform.position = focusPointCurrent + zoomCurrent * trueOffsetAxis;
	}

	protected void StandardUpdate()
	{
		Move();
		Rotate();
		Zoom();
		ResetCamera();

		// Set gameObject position
		transform.position = focusPointCurrent + zoomCurrent * trueOffsetAxis;

		// Look at current view point
		transform.eulerAngles = -offsetAngleCurrent;
	}

	protected void Move()
	{
		if (Input.GetKey(KeyCode.Mouse2))
		{
			focusPointTarget -= (moveSettings.zoomFactor * zoomCurrent + moveSettings.speed) * Input.GetAxis("Mouse Y") * transform.up;
			focusPointTarget -= (moveSettings.zoomFactor * zoomCurrent + moveSettings.speed) * Input.GetAxis("Mouse X") * transform.right;
		}

		focusPointCurrent = Vector3.Lerp(focusPointTarget, focusPointCurrent, moveSettings.smoothing);
	}

	protected void Rotate()
	{
		if (Input.GetKey(KeyCode.Mouse1))
		{
			offsetAngleTarget.x = Mathf.Clamp(offsetAngleTarget.x + rotateSettings.speedVertical * Input.GetAxis("Mouse Y"),
				rotateSettings.verticalLimits.x, rotateSettings.verticalLimits.y);
			offsetAngleTarget.y -= rotateSettings.speedHorizontal * Input.GetAxis("Mouse X");
			// No z axis changes (pitch) for now
		}

		offsetAngleCurrent = Vector3.Lerp(offsetAngleTarget, offsetAngleCurrent, rotateSettings.smoothing);

		MatrixMath();
	}

	protected void MatrixMath()
	{
		Vector3 referenceVector = Vector3.back;	// At all angles 0, the camera should be behind target

		// Calculate rotation about all 3 axes
		Matrix4x4 rotX = new Matrix4x4(
			new Vector4(1, 0, 0, 0),
			new Vector4(0, Mathf.Cos(offsetAngleCurrent.x * Mathf.Deg2Rad), -Mathf.Sin(offsetAngleCurrent.x * Mathf.Deg2Rad), 0),
			new Vector4(0, Mathf.Sin(offsetAngleCurrent.x * Mathf.Deg2Rad), Mathf.Cos(offsetAngleCurrent.x * Mathf.Deg2Rad), 0),
			new Vector4());

		Matrix4x4 rotY = new Matrix4x4(
			new Vector4(Mathf.Cos(offsetAngleCurrent.y * Mathf.Deg2Rad), 0, Mathf.Sin(offsetAngleCurrent.y * Mathf.Deg2Rad), 0),
			new Vector4(0, 1, 0, 0),
			new Vector4(-Mathf.Sin(offsetAngleCurrent.y * Mathf.Deg2Rad), 0, Mathf.Cos(offsetAngleCurrent.y * Mathf.Deg2Rad), 0),
			new Vector4());

		Matrix4x4 rotZ = new Matrix4x4(
			new Vector4(Mathf.Cos(offsetAngleCurrent.z * Mathf.Deg2Rad), -Mathf.Sin(offsetAngleCurrent.z * Mathf.Deg2Rad), 0, 0),
			new Vector4(Mathf.Sin(offsetAngleCurrent.z * Mathf.Deg2Rad), Mathf.Cos(offsetAngleCurrent.z * Mathf.Deg2Rad), 0, 0),
			new Vector4(0, 0, 1, 0),
			new Vector4());

		// Combine rotations in reverse order with the backwards vector
		trueOffsetAxis = rotY * rotX * rotZ * referenceVector;
	}

	// Update zooming variables based on scroll
	protected void Zoom()
	{
		// Input mouse scroll and clamp 0-1
		zoomPercent -= Input.mouseScrollDelta.y * zoomSettings.step;
		zoomPercent = Mathf.Clamp01(zoomPercent);

		// Calculate real zoom distance based on zoom percentage and zoom settings
		zoomTarget = (zoomSettings.farthest - zoomSettings.closest) * Mathf.Pow(zoomPercent, zoomSettings.exponent) + zoomSettings.closest;

		// Lerp current distance to target distance
		zoomCurrent = Mathf.Lerp(zoomTarget, zoomCurrent, zoomSettings.smoothing);
	}

	public void SetZoomDistance(float distance)
	{
		float safeDistance = Mathf.Clamp(distance, zoomSettings.closest, zoomSettings.farthest);
		zoomPercent = Mathf.Pow((safeDistance - zoomSettings.closest) / (zoomSettings.farthest - zoomSettings.closest), 1f / zoomSettings.exponent);
		zoomTarget = (zoomSettings.farthest - zoomSettings.closest) * Mathf.Pow(zoomPercent, zoomSettings.exponent) + zoomSettings.closest;
	}

	protected void ResetCamera()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			focusPointTarget = moveSettings.defaultFocusPoint;
			offsetAngleTarget = rotateSettings.defaultRotation;
			SetZoomDistance(zoomSettings.defaultDistance);
		}
	}

	public float PositiveMod(float dividend, float divisor)
	{
		return ((dividend % divisor) + divisor) % divisor;
	}
}

[Serializable]
public struct MoveSettings
{
	public Vector3 defaultFocusPoint;
	public float speed;		// The constant component of the camera speed (speed = zoomfactor * zoomCurrent + speed)
	public float zoomFactor;	// The linear component of the camera speed based on distance from the focus point
	public float smoothing;	// Value used to Lerp positions, 0 for no lerping, up to 1 for smoother zooming

	public MoveSettings(Vector3 defaultFocusPoint, float speed, float zoomFactor, float smoothing)
	{
		this.defaultFocusPoint = defaultFocusPoint;
		this.speed = speed;
		this.zoomFactor = zoomFactor;
		this.smoothing = smoothing;
	}
}

[Serializable]
public struct RotateSettings
{
	public Vector3 defaultRotation;
	public float speedHorizontal;	// Speed of horizontal rotation (about y axis)
	public float speedVertical;		// Speed of vertical rotation (about x axis)
	public float smoothing;			// Value used to Lerp positions, 0 for no lerping, up to 1 for smoother rotating
	public Vector2 verticalLimits;	// Min and max vertical angles (in degrees)

	public RotateSettings(Vector3 defaultRotation, float speedHorizontal, float speedVertical, float smoothing, Vector2 verticalLimits)
	{
		this.defaultRotation = defaultRotation;
		this.speedHorizontal = speedHorizontal;
		this.speedVertical = speedVertical;
		this.smoothing = smoothing;
		this.verticalLimits = verticalLimits;
	}
}

[Serializable]
public struct ZoomSettings
{
	public float defaultDistance;	// The starting distance the camera will be from the focus point
	public float closest;	// Smallest distance the camera can get from the focus point
	public float farthest;	// Greatest distance the camera can get from the focus point
	public float step;		// Speed of the camera zoom in percentage change per scroll delta
	public float smoothing;	// Value used to Lerp positions, 0 for no lerping, up to 1 for smoother zooming
	public float exponent;	// Exponent to define the zoom curve, higher values zoom faster at larger distances, 1 is linear

	public ZoomSettings(float closest, float farthest, float defaultDistance, float step, float smoothing, float exponent)
	{
		this.closest = closest;
		this.farthest = farthest;
		this.defaultDistance = defaultDistance;
		this.step = step;
		this.smoothing = smoothing;
		this.exponent = exponent;
	}

	/* Practical Limits:
		* -inf < defaultDistance < inf
		* closest >= 0
		* closest < farthest
		* -inf < step < inf, step < 0 for reverse scrolling, step = 0 to disable scrolling
		* 0 <= smoothing < 1, best values around 0.95 - 0.99
		* 0 < exponent < inf, recommended to stay between 1 (linear) and ~10 (highly curved, much faster farther away)
	 */
}