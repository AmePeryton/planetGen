using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GlobalVectors : MonoBehaviour
{
	public int seed;
	public int numPoints;
	[Range(0f, 1f)]
	public float jitter;
	[Range(0, 10)]
	public float frequency;
	public bool remake;
	public SoAGradient gradient = new SoAGradient();
	public float sampleDelta;
	public Vector3 randomPos;
	public Material lineMaterial;
	public float vectorLength;

	[Serializable]
	public class SoAGradient
	{
		public List<Vector3> points = new List<Vector3>();
		public List<float> values = new List<float>();
		public List<GameObject> objects = new List<GameObject>();
		public List<Vector3> vectors = new List<Vector3>();
	}

	void Start()
	{
	}

	void Update()
	{
		if (remake)
		{
			remake = false;

			for (int i = 0; i < gradient.objects.Count; i++)
			{
				Destroy(gradient.objects[i]);
			}
			gradient = new SoAGradient();
			GeneratePoints();
		}
	}

	public float NoiseFunction(Vector3 position)
	{
		// change all noise references from the same function
		return (PerlinNoise.Perlin3D(position, frequency) + 1f) / 2f;
	}

	public void GeneratePoints()
	{
		Random.InitState(seed);
#pragma warning disable CS0618
		seed = Random.seed;
#pragma warning restore CS0618

		float maxJitter = Mathf.Sqrt(2) * Mathf.Pow(Mathf.PI, -Mathf.Log10(numPoints)); // sqrt(2) * pi^(-log_10(n))


		gradient.points.Clear();
		for (int i = 0; i < numPoints; i++)	// fibonacci sphere with jitter
		{
			float g = (1 + Mathf.Sqrt(5)) / 2;
			float theta = 2 * Mathf.PI * i / g;
			float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / numPoints);
			Vector3 defaultPoint = new Vector3(
				Mathf.Cos(theta) * Mathf.Sin(phi),
				Mathf.Cos(phi),
				Mathf.Sin(theta) * Mathf.Sin(phi));
			Vector3 jitterPoint = jitter * maxJitter * Random.onUnitSphere;
			Vector3 newPoint = (defaultPoint + jitterPoint).normalized;

			float newValue = NoiseFunction(newPoint);
			Vector3 newVector = SampleGradient(newPoint);

			gradient.points.Add(newPoint);
			gradient.values.Add(newValue);
			gradient.vectors.Add(newVector);
			gradient.objects.Add(InitializeObject(newPoint, newValue, newVector, ("Ball #" + i)));
		}
	}

	public Vector3 SampleGradient(Vector3 point)
	{
		Vector3 rightAxis = Vector3.Cross(point, Vector3.up).normalized;
		Vector3 upAxis = Vector3.Cross(rightAxis, point).normalized;

		float[] samples = {
			NoiseFunction(point + sampleDelta * rightAxis), // -x
			NoiseFunction(point - sampleDelta * rightAxis), // x
			NoiseFunction(point + sampleDelta * upAxis), 	// -y
			NoiseFunction(point - sampleDelta * upAxis)		// y
		};

		float[] derivitive = { 
			(samples[1] - samples[0]) / sampleDelta,
			(samples[3] - samples[2]) / sampleDelta
		};

		return derivitive[0] * rightAxis + derivitive[1] * upAxis;
	}

	public GameObject InitializeObject(Vector3 position, float value, Vector3 vector, string name)
	{
		GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		newObject.transform.parent = transform;
		newObject.transform.position = position;
		newObject.transform.localScale = 0.01f * Vector3.one;
		newObject.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
		newObject.GetComponent<MeshRenderer>().material.color = ValueToColor(value);
		newObject.name = name;
		LineRenderer lineRenderer = newObject.AddComponent<LineRenderer>();
		lineRenderer.material = lineMaterial;
		lineRenderer.startColor = Color.red;
		lineRenderer.endColor = Color.red;
		lineRenderer.widthMultiplier = 0.01f;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPositions(new Vector3[]{
			position,
			(position + vector * vectorLength)
		});

		newObject.GetComponent<MeshRenderer>().enabled = false;

		return newObject;
	}

	public Color ValueToColor(float value)
	{
		if (value < 0) { return Color.black; }
		if (value <= 0.25f) { return new Color(0, 4*value, 1); }
		if (value <= 0.5f) { return new Color(0, 1, 2-4*value); }
		if (value <= 0.75f) { return new Color(4*value-2, 1, 0); }
		if (value <= 1) { return new Color(1, 4-4*value, 0); }
		return Color.white;
	}
}