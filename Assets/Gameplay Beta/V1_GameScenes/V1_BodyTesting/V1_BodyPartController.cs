using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_BodyPartController : MonoBehaviour
{
	public V1_BodyPartData data;

	public bool physicsEnabled;

	[Header("Prefabs")]
	public Mesh cubeMesh;
	public Mesh sphereMesh;
	public Mesh cylinderMesh;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private new Collider collider;
	private new Rigidbody rigidbody;

	private void Awake()
	{
		BodyPartInit();
	}

	void Start()
	{
		
	}

	void Update()
	{
		UpdatePhysics();
		UpdateDisplay();
	}

	public void BodyPartInit()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();

		// Primitive colliders can't change shape, so destroy old one and make new one
		// Mesh colliders can change shape, but with performance cost (and might not have same functions)
		if (collider != null)
		{
			Destroy(collider);
		}
		collider = null;

		switch (data.shape)
		{
			case BodyPartShape.Cube:
				meshFilter.mesh = cubeMesh;
				collider = gameObject.AddComponent<BoxCollider>();
				break;
			case BodyPartShape.Sphere:
				meshFilter.mesh = sphereMesh;
				collider = gameObject.AddComponent<SphereCollider>();
				break;
			case BodyPartShape.Cylinder:
				meshFilter.mesh = cylinderMesh;
				collider = gameObject.AddComponent<CapsuleCollider>();
				break;
			default:
				break;
		}
	}

	public void SetPhysics(bool p)
	{
		physicsEnabled = p;
	}

	public void SetPosition(Vector3 position)
	{
		data.position = position;
	}

	public void SetRotation(Vector3 rotation)
	{
		data.rotation = rotation;
	}

	public void SetScale(Vector3 scale)
	{
		data.scale = new Vector3(Mathf.Clamp(scale.x, 0, float.PositiveInfinity),
			Mathf.Clamp(scale.y, 0, float.PositiveInfinity),
			Mathf.Clamp(scale.z, 0, float.PositiveInfinity));
	}

	public void DestroyBodyPart()
	{

	}

	public void UpdatePhysics()
	{
		rigidbody.mass = data.mass;
		rigidbody.useGravity = physicsEnabled;
		rigidbody.isKinematic = !physicsEnabled;
	}

	public void UpdateDisplay()
	{
		if (!physicsEnabled)
		{
			transform.position = data.position;
			transform.eulerAngles = data.rotation;
		}
		transform.localScale = data.scale;
	}
}

public enum BodyPartShape
{
	Cube,
	Sphere,
	Cylinder
}

[Serializable]
public class V1_BodyPartData
{
	public BodyPartShape shape;
	public Vector3 position;
	public Vector3 rotation;
	public Vector3 scale;
	public float mass;
	public float volume;

	public List<V1_JointData> joints;
}