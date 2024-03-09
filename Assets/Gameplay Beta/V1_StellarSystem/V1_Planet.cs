using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Planet : MonoBehaviour, ISavableData
{
	public float mass;
	public float radius;
	public float distance;
	public GameObject sphere;

	void Start()
	{
		mass = Random.value;
		radius = Random.value;
		distance = 10f * Random.value;
		transform.localPosition = new Vector3(0, 0, distance);
		sphere.transform.localScale = radius * Vector3.one;
	}

	void Update()
	{
	}

	public void LoadData(V1_SaveData data)
	{
		Debug.Log("planet data loaded!");
		mass = data.planetData.mass;
		radius = data.planetData.radius;
		distance = data.planetData.distance;
	}

	public void SaveData(ref V1_SaveData data)
	{
		data.planetData.mass = mass;
		data.planetData.radius = radius;
		data.planetData.distance = distance;
	}
}