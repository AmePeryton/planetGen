using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// applied to a rigidbody of an object which should be affected by other objects' gravitational pulls
// just recieves a call when a gravity trigger detects this rigidbody enters / exits it, then enrolls / unenrolls
public class GravityEnroller : MonoBehaviour
{
	public GravitySimulator gravityScript;

	// adds the calling object to this object's list of gravity sources, if its a different object
	public void Enroll(GravitySimulator callingScript)
	{
		if (callingScript != gravityScript && !gravityScript.gravitySources.Contains(callingScript))
		{
			//Debug.Log(gravityScript.parentObject.name + " entered " + callingScript.parentObject.name + "'s gravity trigger");
			gravityScript.gravitySources.Add(callingScript);
			callingScript.gravityInfluences.Add(gravityScript);
		}
	}

	public void UnEnroll(GravitySimulator callingScript)
	{
		if (gravityScript.gravitySources.Contains(callingScript))
		{
			//Debug.Log(gravityScript.parentObject.name + " exited " + callingScript.parentObject.name + "'s gravity trigger");
			gravityScript.gravitySources.Remove(callingScript);
			callingScript.gravityInfluences.Remove(gravityScript);
		}
	}
}