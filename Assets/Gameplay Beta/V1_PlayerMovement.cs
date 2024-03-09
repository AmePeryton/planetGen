using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_PlayerMovement : MonoBehaviour
{
	public float forceScalar;
	public Transform lookDirection;
	public float horizontalInput;
	public float verticalInput;
	public Vector3 moveDirection;
	public Rigidbody rigid;


	void Start()
	{
		rigid = GetComponent<Rigidbody>();
		//rigid.freezeRotation = true;
	}

	void Update()
	{
		MyInput();
	}

	void FixedUpdate()
	{
		MovePlayer();
		RotationFollowsDirection();
	}

	private void MyInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");
	}

	private void MovePlayer()
	{
		moveDirection = lookDirection.forward * verticalInput + lookDirection.right * horizontalInput;
		rigid.AddForce(moveDirection.normalized * forceScalar, ForceMode.Force);
	}

	private void RotationFollowsDirection()
	{
		if (rigid.velocity.magnitude > 0.1f)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rigid.velocity), 0.2f);
		}
	}
}