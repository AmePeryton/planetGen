using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// control script for a planetesimal, communicating between all other subscripts and setting cosmetic values
public class PlanetesimalProperties : MonoBehaviour
{
	public GameObject planetesimalPrefab;
	public GravitySimulator gravityScript;
	public PlanetesimalCollisionDetector collisionScript;
	public GameObject visualSphere;
	public GameObject collisionSphere;
	public GameObject gravityTrigger;
	public GameObject debugObject;
	public GameObject gravityForceIndicator;
	public GameObject dragForceIndicator;
	public GameObject velocityIndicator;
	public SphereCollider collisionCollider;
	public TrailRenderer trail;
	public float mass;		// in kg
	public float radiuskm;  // in km
	public float radiusAU;  // in AU
	public float timeScale;
	public float visualScale;
	public bool visualEffects;
	public bool debugEffects;
	// TODO: Add component element stats:
		// Hydrogen (H2)
		// Water (H2O)
		// Silicate Rocks
		// Methane?
		// Metals
		// Ammonia?

	private float gravConstant = 6.6743f * Mathf.Pow(10, -11);
	private float earthMassToKG = 5.972f * Mathf.Pow(10, 24);
	private float AUtoKM = 1.496f * Mathf.Pow(10, 8);
	private float density = 5000f;

	[Header ("Statistics")]
	public float earthMasses;
	public float orbitalDistance;   // in AU
	public int numOrbits;

	void Start()
	{
		// disable subscripts until all values are set
		gravityTrigger.SetActive(false);
		collisionSphere.SetActive(false);
		gravityScript.mass = mass;
		gravityTrigger.SetActive(true);
		collisionSphere.SetActive(true);
	}

	void Update()
	{
		gravityScript.mass = mass;
		gravityScript.timeScale = timeScale;
		earthMasses = mass / earthMassToKG;
		radiuskm = Mathf.Pow((3 * mass) / (4 * Mathf.PI * density), 1f / 3f) / 1000f;
		radiusAU = radiuskm / AUtoKM;
		orbitalDistance = transform.position.magnitude; // NOTE: Make this more independent of center
		VisualUpdate();
	}

	public void VisualUpdate()
	{
		visualSphere.transform.localScale = radiusAU * Vector3.one * visualScale * 2; // spheres at scale 1 have a radius of 0.5
		if (visualEffects)
		{
			trail.enabled = true;
			//trail.widthMultiplier = earthMasses;
		}
		else
		{
			trail.enabled = false;
			trail.Clear();
		}

		if(debugEffects)
		{
			debugObject.SetActive(true);
			if(gravityScript.gravForceVector.magnitude > Mathf.Pow(10, 17))
			{
				gravityForceIndicator.transform.localScale = new Vector3(1, 1, 
					Mathf.Log10(gravityScript.gravForceVector.magnitude * Mathf.Pow(10, -17)) / 10);
			}
			else
			{
				gravityForceIndicator.transform.localScale = Vector3.zero;
			}
			gravityForceIndicator.transform.LookAt(transform.position + (gravityScript.gravForceVector).normalized);
			if(gravityScript.dragForce.magnitude > Mathf.Pow(10, 15))
			{
				dragForceIndicator.transform.localScale = new Vector3(1, 1, 
					Mathf.Log10(gravityScript.dragForce.magnitude * Mathf.Pow(10, -15)) / 10);
			}
			else
			{
				dragForceIndicator.transform.localScale = Vector3.zero;
			}
			dragForceIndicator.transform.LookAt(transform.position + (gravityScript.dragForce).normalized);
			velocityIndicator.transform.localScale = new Vector3(1, 1, timeScale / (AUtoKM * 60000) * gravityScript.velocityVector.magnitude);
			velocityIndicator.transform.LookAt(transform.position + gravityScript.velocityVector);
		}
		else
		{
			debugObject.SetActive(false);
			debugObject.GetComponent<TrailRenderer>().Clear();
		}
	}

	public void CollisionEvent(PlanetesimalProperties otherScript)
	{
		if (mass > otherScript.mass)
		{
			transform.position = Vector3.Lerp(
				transform.position,
				otherScript.transform.position,
				mass / (mass + otherScript.mass));
			//gravityScript.CollisionVelocity(otherScript.gravityScript.mass, otherScript.gravityScript.velocityVector);
			gravityScript.velocityVector = (mass * gravityScript.velocityVector + otherScript.mass * otherScript.gravityScript.velocityVector) / (mass + otherScript.mass);
			mass += otherScript.mass;
			otherScript.SelfDestruct(true, "Collided with " + gameObject.name);
			name += " & " + otherScript.name;
			gravityScript.gravitySources.RemoveAll(i => i == null);
		}
	}

	public void SelfDestruct(bool printMessage, string infoMessage)
	{
		if (printMessage)
		{
			Debug.Log(gameObject.name + " was destroyed: " + infoMessage);
		}
		foreach (GravitySimulator source in gravityScript.gravitySources)
		{
			source.gravityInfluences.Remove(gravityScript);
		}
		foreach (GravitySimulator influence in gravityScript.gravityInfluences)
		{
			influence.gravitySources.Remove(gravityScript);
		}
		Destroy(gameObject);
	}
}