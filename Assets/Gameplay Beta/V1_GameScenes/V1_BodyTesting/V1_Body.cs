using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Body : MonoBehaviour
{
	public HashSet<V1_BodyPartController> bodyParts;

	[Header("Prefabs")]
	public GameObject bodyPartPrefab;

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	public void AddBodyPart()
	{
		bodyParts.Add(Instantiate(bodyPartPrefab).GetComponent<V1_BodyPartController>());
	}
}