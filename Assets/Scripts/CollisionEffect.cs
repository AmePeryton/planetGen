using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
	public float lifetime;
	public float scale;

	private float currScale;
	private float currTime;

	void Start()
	{
		currScale = 0;
		currTime = 0;
	}

	void Update()
	{
		Decay();
	}

	void Decay()
	{
		currTime += Time.deltaTime;
		if(currTime >= lifetime)
		{
			Destroy(gameObject);
		}

		currScale = scale * (1 - Mathf.Pow(((2 * currTime / lifetime) - 1), 2));

		transform.localScale = new Vector3(currScale, currScale, currScale);
	}
}