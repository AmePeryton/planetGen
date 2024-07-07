using UnityEngine;

public class V1_OrganNode : MonoBehaviour
{
	public V1_OrganType type;
	public Vector3 position;

	private Material material;

	private void Awake()
	{
		material = GetComponent<Renderer>().material;
	}

	void Start()
	{
		
	}

	void Update()
	{
		transform.position = position;
		material.color = type.color;
	}
}