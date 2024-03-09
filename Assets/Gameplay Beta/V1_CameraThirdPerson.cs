using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_CameraThirdPerson : MonoBehaviour
{
	public float sensX;
	public float sensY;
	public float xRotation;
	public float yRotation;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
		float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
	}
}