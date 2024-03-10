using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Star : MonoBehaviour, IGameSavableData
{
	public float mass;
	public float age;
	public GameObject sphere;

	void Start()
	{
		mass = Random.value;
		age = Random.value;
		sphere.transform.localScale = Mathf.Pow(mass, 0.74f) * Vector3.one;
	}

	void Update()
	{
	}

	public void LoadData(V1_GameSaveData data)
	{
		Debug.Log("star data loaded!");
		mass = data.starData.mass;
		age = data.starData.age;
	}

	public void SaveData(ref V1_GameSaveData data)
	{
		data.starData.mass = mass;
		data.starData.age = age;
	}
}