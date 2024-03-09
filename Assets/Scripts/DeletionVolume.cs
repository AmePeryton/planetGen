using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionVolume : MonoBehaviour
{
	private void OnTriggerExit(Collider other)
	{
		PlanetesimalCollisionDetector otherScript;
		if (other.gameObject.TryGetComponent<PlanetesimalCollisionDetector>(out otherScript))
		{
			otherScript.propertiesScript.SelfDestruct(true, "Exited the deletion volume");
		}
	}
}