using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// only simulates physics for the body and manages gravity sources. can be used on many different types of bodies
// apply this script to an empty child of the body with a sphere trigger collider representing the sphere of influence
public class GravitySimulator : MonoBehaviour
{
	public GameObject parentObject;	// object to physically move, normally the parent gameobject
	public List<GravitySimulator> gravitySources = new List<GravitySimulator>();
	public List<GravitySimulator> gravityInfluences = new List<GravitySimulator>();	// reverse of gravitySources
	public float mass;  // in Earth Masses, copy from parent script	NOTE: Convert to KG
	public Vector3 gravForceVector; // in Newtons (kg * m/s^2)
	public Vector3 accelVector; // in m/s^2
	public Vector3 velocityVector; // in m/s
	public Vector3 dragForce;
	public SphereCollider gravityTrigger;
	public float timeScale; // speed physics is simulated, normally set by parent script
	public float dragDensity; // the ammount of drag a body will feel from this object, mostly to simulate dust around planetesimals

	private float gravConstant = 6.6743f * Mathf.Pow(10, -11);  // m^3 / (kg * s^2)
	private float earthMassToKG = 5.972f * Mathf.Pow(10, 24);   // kg / E_m
	private float AUtoM = 1.496f * Mathf.Pow(10, 11);           // m / AU

	void Update()
	{
		gravityTrigger.radius = Mathf.Clamp(mass / earthMassToKG * 0.1f, 0f, 1000f);
	}

	private void FixedUpdate()
	{
 		gravForceVector = Vector3.zero;
		dragForce = Vector3.zero;

		gravitySources.RemoveAll(i => i == null);
		foreach (GravitySimulator source in gravitySources)
		{
			Vector3 thisForce = (source.parentObject.transform.position - parentObject.transform.position).normalized;    // simply directional
			double tempForceMagnitude = (mass * source.mass * gravConstant); // super big numbers, so must be stored as a double
			tempForceMagnitude /= Mathf.Pow((Vector3.Distance(parentObject.transform.position, source.parentObject.transform.position) * AUtoM), 2);
			thisForce *= (float)tempForceMagnitude;
			gravForceVector += thisForce;

			Vector3 velocityDifference = source.velocityVector - velocityVector;
			dragForce += 0.47f * source.dragDensity * Vector3.Scale(velocityDifference, velocityDifference) * 1.278f * Mathf.Pow(10, 14);
		}
		accelVector = (gravForceVector + dragForce) / mass;
		velocityVector += accelVector * timeScale * Time.deltaTime;
		parentObject.transform.position += (velocityVector / AUtoM) * timeScale * Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		GravityEnroller otherEnroller;
		if (other.TryGetComponent<GravityEnroller>(out otherEnroller))
		{
			otherEnroller.Enroll(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		GravityEnroller otherEnroller;
		if (other.TryGetComponent<GravityEnroller>(out otherEnroller))
		{
			otherEnroller.UnEnroll(this);
		}
	}

	public void CollisionVelocity(float otherMass, Vector3 otherVelocity)
	{
		velocityVector = (mass * velocityVector + otherMass * otherVelocity) / (mass + otherMass);
	}

}