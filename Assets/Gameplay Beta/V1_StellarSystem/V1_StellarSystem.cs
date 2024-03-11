using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_StellarSystem : MonoBehaviour
{
	public GameObject starPrefab;
	public GameObject planetPrefab;

	private void Awake()
	{
		Instantiate(starPrefab);
		Instantiate(planetPrefab);
	}

	void Start()
	{
	}

	void Update()
	{
		
	}
}