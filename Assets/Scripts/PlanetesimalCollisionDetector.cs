using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simply passes the other planetesimal's main script to its own main script, which handles the behavior
public class PlanetesimalCollisionDetector : MonoBehaviour
{
	public PlanetesimalProperties propertiesScript;  // script that actually manages the effect of the collision
	public SphereCollider thisCollider;
	public float collisionScale;	// the colliders are so small and the timescale so fast that they need to be larger
	// so that the colliders do not pass each other when they get too close and fast
	// However, slingshotting does occur irl, so sometimes they should actually pass eachother

	private void FixedUpdate()
	{
		thisCollider.radius = propertiesScript.radiusAU * collisionScale;
	}

	private void OnTriggerEnter(Collider other)
	{
		PlanetesimalCollisionDetector otherScript;
		if (other.gameObject.TryGetComponent<PlanetesimalCollisionDetector>(out otherScript))
		{
			propertiesScript.CollisionEvent(otherScript.propertiesScript);
		}
	}
}