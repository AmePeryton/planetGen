using System;
using System.Collections.Generic;
using UnityEngine;

public class V1_BodyPartController : MonoBehaviour
{
	public V1_BodyPartData data;
	public bool physicsEnabled;
	public List<V1_JointController> jointControllers;

	[Header("Prefabs")]
	public Mesh cubeMesh;
	public Mesh sphereMesh;
	public Mesh cylinderMesh;
	public GameObject bodyPartPrefab;
	public GameObject jointPrefab;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private new Collider collider;
	private new Rigidbody rigidbody;

	private void Awake()
	{

	}

	void Start()
	{
		BodyPartInit();
	}

	void Update()
	{
		UpdatePhysics();
		UpdateDisplay();
	}

	public void BodyPartInit()
	{
		name = "Body Part " + V1_BodyController.partCounter++;

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

		foreach (V1_JointData joint in data.joints)
		{
			AddBodyPart(joint);
		}
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

	public void AddNewBodyPart()
	{
		// Instantiate new joint and set new data
		V1_JointController newJoint = Instantiate(jointPrefab).GetComponent<V1_JointController>();
		newJoint.data = new V1_JointData();

		// Instantiate new body part and set new data
		V1_BodyPartController newBodyPart = Instantiate(bodyPartPrefab).GetComponent<V1_BodyPartController>();
		newBodyPart.data = new V1_BodyPartData();
		newJoint.data.jointedPart = newBodyPart.data;

		// Update joint controller
		newJoint.parentController = this;
		newJoint.childController = newBodyPart;

		// Update joint controllers list
		jointControllers.Add(newJoint);

		// Add new joint to this body part
		data.joints.Add(newJoint.data);

		// Joint controller adds component to parent, and sets attached body as the child
	}

	public void AddBodyPart(V1_JointData joint)
	{
		// Instantiate joint and set data
		V1_JointController newJoint = Instantiate(jointPrefab).GetComponent<V1_JointController>();
		newJoint.data = joint;

		// Instantiate body part and set data
		V1_BodyPartController newBodyPart = Instantiate(bodyPartPrefab).GetComponent<V1_BodyPartController>();
		newBodyPart.data = joint.jointedPart;

		// Update joint controller
		newJoint.parentController = this;
		newJoint.childController = newBodyPart;

		// Update joint controllers list
		jointControllers.Add(newJoint);

		// Joint controller adds component to parent, and sets attached body as the child
	}

	public void DestroyBodyPart()
	{
		foreach (V1_JointController joint in jointControllers)
		{
			joint.childController.DestroyBodyPart();
			Destroy(joint.gameObject);
		}

		V1_BodyController.partCounter = 0;
		Destroy(gameObject);
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

	public V1_BodyPartData()
	{
		shape = BodyPartShape.Cube;
		position = Vector3.zero;
		rotation = Vector3.zero;
		scale = Vector3.one;
		mass = 1.0f;
		volume = 1.0f;
		joints = new List<V1_JointData>();
	}
}