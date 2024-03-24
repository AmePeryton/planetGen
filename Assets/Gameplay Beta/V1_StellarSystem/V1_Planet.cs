using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Planet : MonoBehaviour
{
	public float mass;
	public float radius;
	public float distance;
	public GameObject sphere;

	private void Awake()
	{
	}

	void Start()
	{
	}

	void Update()
	{
		VisualUpdate();
		// NOTE: OK for early development, but later the constant refreshing will cause low fps
	}

	private void VisualUpdate()
	{
		transform.localPosition = new Vector3(0, 0, distance);
		sphere.transform.localScale = radius * Vector3.one;
	}

	public void RandomizeProperties()
	{
		mass = Random.value;
		radius = Random.value;
		distance = 5f * Random.value;
		VisualUpdate();
	}
}