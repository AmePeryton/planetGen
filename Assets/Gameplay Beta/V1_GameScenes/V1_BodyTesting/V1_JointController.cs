using System;
using UnityEngine;

public class V1_JointController : MonoBehaviour
{
	public V1_JointData data;
	public V1_BodyPartController parentController;
	public V1_BodyPartController childController;

	public ConfigurableJoint jointComponent;

	void Start()
	{
		JointInit();
	}

	public void JointInit()
	{
		name = "Joint " + V1_BodyController.partCounter;
	}
}

public enum JointType
{
	Fixed,	// No rotation
	Hinge,	// Rotates on 1 axis, perpedicular to primary axis
	Sadle,	// Rotates on 2 axes, both perpendicular to primary axis
	Pivot,	// Rotates on 1 axis, the primary axis
	Ball	// Rotates on all 3 axes
}

[Serializable]
public class V1_JointData
{
	public JointType jointType;	// Type of joint
	public Vector3 position;	// Position of the anchor in the parent body part's local space
	public Vector3 primaryAxis;	// Axis of the joint component
	public Vector3 secondaryAxis;	// Secondary Axis of the joint component
	public V1_BodyPartData jointedPart;	// Paired body part

	public V1_JointData(JointType j = JointType.Fixed, Vector3 p = new Vector3(), V1_BodyPartData a = null)
	{
		jointType = j;
		position = p;
		primaryAxis = Vector3.right;
		secondaryAxis = Vector3.up;
		jointedPart = a;
	}
}