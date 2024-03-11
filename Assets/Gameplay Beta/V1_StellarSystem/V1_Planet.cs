using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Planet : MonoBehaviour, IGameSavableData
{
	public float mass;
	public float radius;
	public float distance;
	public GameObject sphere;

	private void Awake()
	{
		mass = Random.value;
		radius = Random.value;
		distance = 5f * Random.value;
		VisualUpdate();
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

	public void LoadGameSaveData(V1_GameSaveData data)
	{
		mass = data.planetData.mass;
		radius = data.planetData.radius;
		distance = data.planetData.distance;
		VisualUpdate();
	}

	public void SaveGameSaveData(ref V1_GameSaveData data)
	{
		data.planetData.mass = mass;
		data.planetData.radius = radius;
		data.planetData.distance = distance;
	}
}