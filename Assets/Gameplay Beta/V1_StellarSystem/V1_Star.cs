using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Star : MonoBehaviour//, IGameSavableData
{
	public float mass;
	public float age;
	public GameObject sphere;

	private void Awake()
	{
		mass = Random.value;
		age = Random.value;
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
		sphere.transform.localScale = Mathf.Pow(mass, 0.74f) * Vector3.one;
	}

	//public void LoadGameSaveData(V1_GameSaveData data)
	//{
	//	mass = data.starData.mass;
	//	age = data.starData.age;
	//	VisualUpdate();
	//}

	//public void SaveGameSaveData(ref V1_GameSaveData data)
	//{
	//	data.starData.mass = mass;
	//	data.starData.age = age;
	//}
}