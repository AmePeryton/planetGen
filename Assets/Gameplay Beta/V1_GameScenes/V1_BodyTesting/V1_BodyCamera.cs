public class V1_BodyCamera : V1_ThirdPersonCameraController
{
	private void Awake()
	{
		CameraInit();
	}

	private void Update()
	{
		Move();
		Rotate();
		Zoom();

		// Set gameObject position
		transform.position = focusPointCurrent + zoomCurrent * trueOffsetAxis;

		// Look at current view point
		transform.eulerAngles = -offsetAngleCurrent;
	}
}