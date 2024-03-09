using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_PlayerStats : MonoBehaviour
{
	[Header ("Stats")]
	public float oxygen;
	public float nutrition;
	public float water;
	public float health;

	[Header("Stat Depletion / Healing Rates")]
	public float oxygenRate;
	public float nutritionRate;
	public float waterRate;
	public float healthRate;

	void Start()
	{
		oxygen = 1;
		nutrition = 1;
		water = 1;
		health = 1;
	}

	void Update()
	{
		oxygen = Mathf.Clamp(oxygen - oxygenRate, 0, 1);
		nutrition = Mathf.Clamp(nutrition - nutritionRate, 0, 1);
		water = Mathf.Clamp(water - waterRate, 0, 1);
		health = Mathf.Clamp(health + healthRate, 0, 1);
	}
}