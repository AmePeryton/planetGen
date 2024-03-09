using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_StellarSystem : MonoBehaviour
{
	public GameObject starPrefab;
	public GameObject planetPrefab;

	void Start()
	{
		Instantiate(starPrefab);
		Instantiate(planetPrefab);
	}

	void Update()
	{
		
	}
}