using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Body : MonoBehaviour
{
	public V1_BodyPartData data;
	public V1_BodyPartController selectedPart;

	[Header("Prefabs")]
	public GameObject bodyPartPrefab;
	
	private void Awake()
	{

	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}
}

public class V1_BodyPartData
{
	public V1_BodyPartData coreBodyPart;	// The root of the body part tree, connected by joint
	//public HashSet<V1_BodyPartController> bodyParts;

	public V1_BodyPartData()
	{
		coreBodyPart = null;
	}
}