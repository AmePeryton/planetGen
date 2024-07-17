public class V1_BodyCamera : V1_ThirdPersonCameraController
{
	private void Awake()
	{
		CameraInit();
	}

	private void Update()
	{
		StandardUpdate();
	}
}