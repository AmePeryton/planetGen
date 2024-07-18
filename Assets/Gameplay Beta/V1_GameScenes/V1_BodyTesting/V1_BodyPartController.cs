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
		// Set components
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();

		// Disable this gameobject until it is initialized by the creating script
		//gameObject.SetActive(false);	// since awake calls twice sometimes, this won't work right
	}

	void Update()
	{
		UpdatePhysics();
		UpdateDisplay();
	}

	public void BodyPartInit(V1_BodyPartData initData)
	{
		// Set data as reference to passed initData
		data = initData;

		// Set name
		name = "Body Part " + ++V1_BodyController.partCounter;

		// Set shape related values
		SetShape(initData.shape);

		// Add joint controllers and body part controllers
		foreach (V1_JointData joint in data.joints)
		{
			AddJointedBodyPart(joint);
		}

		// Re-enable self
		gameObject.SetActive(true);
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

	public void SetShape(BodyPartShape newShape)
	{
		data.shape = newShape;

		// Primitive colliders can't change shape, so destroy old one and make new one
		// Mesh colliders can change shape, but with performance cost (and might not have same functions)
		if (collider != null)
		{
			Destroy(collider);
		}
		collider = null;

		switch (newShape)
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
				Debug.LogWarning("SetShape defaulted");
				meshFilter.mesh = null;
				collider = null;
				break;
		}
	}

	public void CreateNewBodyPart()
	{
		// Initilize data
		V1_JointData newJointData = new V1_JointData
		{
			jointedPart = new V1_BodyPartData()
		};

		// Add new joint to this body part
		data.joints.Add(newJointData);

		// Set up controllers for new part and joint
		AddJointedBodyPart(newJointData);
	}

	public void AddJointedBodyPart(V1_JointData joint)
	{
		// Instantiate body part and set data
		V1_BodyPartController newBodyPart = Instantiate(bodyPartPrefab).GetComponent<V1_BodyPartController>();
		newBodyPart.Awake();	// Awake not called automatically when instantiating script of same type
		newBodyPart.BodyPartInit(joint.jointedPart);

		// Instantiate joint and set data
		V1_JointController newJoint = Instantiate(jointPrefab).GetComponent<V1_JointController>();
		newJoint.transform.parent = transform;	// BUG: duplicate joints get added to all parts if...
		//the original joint's transform is parented to something
		newJoint.JointInit(joint, this, newBodyPart);

		// Update joint controllers list
		jointControllers.Add(newJoint);
	}

	public void DestroyBodyPart()
	{
		foreach (V1_JointController joint in jointControllers)
		{
			joint.childController.DestroyBodyPart();
			Destroy(joint.gameObject);
		}

		V1_BodyController.partCounter--;
		Debug.Log(V1_BodyController.partCounter);
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