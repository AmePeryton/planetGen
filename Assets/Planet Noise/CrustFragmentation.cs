using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using UnityEngine.Rendering;
using System.Linq;
using System.Threading.Tasks;
using System;

public class CrustFragmentation : MonoBehaviour
{
	public bool reload;
	public int numVertices;
	[Range(0f, 1f)]
	public float jitter;
	public int seed;
	public bool multiMesh;
	public bool project;

	[Header("Triangulation Data")]
	public Delaunator delaunator;
	public List<Vector3> vertices = new List<Vector3>();
	public List<int> edges = new List<int>();
	public List<int> triangles = new List<int>();
	public List<GameObject> meshObjects;
	public GameObject projectedMesh;
	public Material semiTransparent;

	public int search;
	public List<GameObject> myBals = new List<GameObject>();

	[Header("Crust")]
	public List<CrustSegment> crustSegments = new List<CrustSegment>();

	[Header("Randomized Direction Vectors")]
	public List<GameObject> triCenters = new List<GameObject>();

	public class CrustSegment
	{
		public Vector3[] vertices;	// position of the 3 vertices of this triangle
		public Vector3 velocity;	// velocity sampled from the average position of the vertices
		public CrustSegment[] neighborSegments;	// 3 neighboring triangles (edge sharers)

		public CrustSegment(Vector3[] vertices, Vector3 velocity, CrustSegment[] neighborSegments)
		{
			this.vertices = vertices;
			this.velocity = velocity;
			this.neighborSegments = neighborSegments;
		}
	}

	public class CrustPlate
	{
		public List<CrustSegment> segments;
		public Vector3 velocity;

		public CrustPlate(List<CrustSegment> segments, Vector3 velocity)
		{
			this.segments = segments;
			this.velocity = velocity;
		}
	}

	void Start()
	{
		
	}

	void Update()
	{
		if (reload)
		{
			reload = false;
			GenerateVertices();
			Triangulate();
			PairEdges();
			foreach (GameObject oldMesh in meshObjects)
			{
				Destroy(oldMesh);
			}
			meshObjects.Clear();
			if (multiMesh)
			{
				for (int i = 0; i < triangles.Count; i += 3)
				{
					meshObjects.Add(BuildMesh(new List<int> { triangles[i], triangles[i+1], triangles[i+2] }, "Mesh " + (i/3)));
				}
			}
			else
			{
				meshObjects.Add(BuildMesh(triangles, "Mesh"));
			}
			Destroy(projectedMesh);
			if (project)
			{
				BuildProjectedMesh();
			}
			SurfaceMovement();
		}
	}

	// Fills the vertices list with specified number of vector3s on the sphere
	public void GenerateVertices()
	{
		Random.InitState(seed);
#pragma warning disable CS0618
		seed = Random.seed;
#pragma warning restore CS0618

		float maxJitter = Mathf.Sqrt(2) * Mathf.Pow(Mathf.PI, -Mathf.Log10(numVertices));	// sqrt(2) * pi^(-log_10(n))

		vertices.Clear();
		for (int i = 0; i < numVertices; i++)	// fibonacci sphere with jitter
		{
			float g = (1 + Mathf.Sqrt(5)) / 2;
			float theta = 2 * Mathf.PI * i / g;
			float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / numVertices);
			Vector3 newpoint = new Vector3(
				Mathf.Cos(theta) * Mathf.Sin(phi),
				Mathf.Cos(phi),
				Mathf.Sin(theta) * Mathf.Sin(phi));
			Vector3 jitterPoint = jitter * maxJitter * Random.onUnitSphere;

			vertices.Add((newpoint + jitterPoint).normalized);
		}
	}

	// Calculates the delaunay triangulation of the points
	public void Triangulate()
	{
		float startTime = Time.realtimeSinceStartup;

		triangles.Clear();	// remove old triangulation
		List<Vector2> pointsProjection = new List<Vector2>();
		foreach (Vector3 vertex in vertices)
		{
			pointsProjection.Add(StereographicProjection(vertex, true));
		}
		delaunator = new Delaunator(pointsProjection.ToPoints());   // main delaunator
		triangles = delaunator.Triangles.ToList();	// add main delaunator's triangulation to our full triangulation
		pointsProjection.Clear();	// remove previous projected points

		foreach (int point in delaunator.Hull)
		{
			pointsProjection.Add(StereographicProjection(vertices[point], false));	// prepare south pole points for triangulation
		}
		Delaunator hullDelaunator = new Delaunator(pointsProjection.ToPoints());	// triangulate hole at south pole
		int[] validHullTriangles = ValidateHull(hullDelaunator);	// fix triangles at south pole to be completely valid with main triangles
		foreach (int tri in validHullTriangles)
		{
			triangles.Add(delaunator.Hull[tri]);	// combine main triangles and hole correction triangles
		}

		//OopsAllBalls();		// put balls on all points

		Debug.Log("Triangulation: " + (Time.realtimeSinceStartup - startTime));
	}

	// Project a 3d point into 2d space
	private Vector2 StereographicProjection(Vector3 position, bool north)
	{
		if (north)
		{
			return new Vector2(position.x / (1 + position.y), position.z / (1 + position.y));
		}
		return new Vector2(-position.x / (1 - position.y), position.z / (1 - position.y));
	}

	// Modifies hull triangulation so that they match up wth existing main tris, with no extra tris
	private int[] ValidateHull(Delaunator del)
	{
		float startTime = Time.realtimeSinceStartup;
		List<int> tempTriangles = del.Triangles.ToList();	// working tris, so bad tris can be removed
		List<int> tempHalfedges = del.Halfedges.ToList();	// working halfedges, so references to removed edges can be changed

		for (int i = 0; i < tempTriangles.Count/3; i++)	// foreach tri (3 verts) in hull triangulation:
		{
			int[] tvs = new int[]{
				tempTriangles[3 * i],
				tempTriangles[3 * i + 1], 
				tempTriangles[3 * i + 2]};	// the vertexes in this tri

			for (int j = 0; j < 3; j++)	// for each edge (pair of verts) in the tri:
			{
				if (tempHalfedges[3 * i + j] == -1)	// if half edge has no opposite (in the hull)
				{
					// (([next vert in tri] + 1) % num points), because valid border edges are (n+1, n) or (0, numverts)
					if ((tvs[(j+1) % 3] + 1) % del.Points.Length != tvs[j])	// if this edge is on the border but not using a valid hull edge
					{
						tempTriangles.RemoveRange(3 * i, 3);	// remove these tris from fixed triangulation
						tempHalfedges.RemoveRange(3 * i, 3);	// remove halfedges for this tri from the working halfedges
						for (int k = 0; k < tempHalfedges.Count; k++)	// fix working halfedges list
						{
							if (tempHalfedges[k] >= 3 * i && tempHalfedges[k] <= 3 * i + 2)	// if referenced halfedge is one previously deleted
							{
								tempHalfedges[k] = -1;	// set this edge as having no opposite
							}
						}
						i--;	// back up counter, since this tri was removed
						break;
					}
				}
			}
		}

		Debug.Log("Hull Validation: " + (Time.realtimeSinceStartup - startTime));
		return tempTriangles.ToArray();
	}

	// converts triangulation to list of edges with affixed indices
	// a,b,c,d,e,f ==> 0,a,b,0,b,c,0,c,a,1,d,e,1,e,f,1,f,d
	private List<int> TrisToEdges(List<int> tris)
	{
		List<int> output = new List<int>();
		for (int i = 0; i < tris.Count; i += 3)
		{
			int[] tempEdges = { i/3, tris[i], tris[i+1], 
								i/3, tris[i+1], tris[i+2], 
								i/3, tris[i+2], tris[i]};
			output.AddRange(tempEdges);
		}
		return output;
	}

	/*(on the premise that for every unidirectional edge in the triangulation, 
		there exists its reverse in the triangulation in some other triangle.)
	1. Get edges list from triangulation (so that it is able to withstand deletions more than the triangulation)
	2. do until edgesList is empty:
		2a. take first 3 entries in the list (the first edge + its triangle index)
		2b. invalidate this edge (set tri index to -1)
		2c. iterate through every edge in edgesList looking for the edge's inverse in valid edges
			2ci. if found, add the first edge's tri index and the found edge's tri index to the official edges list
			2cii. break loop
	*/
	// PairEdgesOld but with invalidating instead of removal, significantly faster
	private async void PairEdges()
	{
		float startTime = Time.realtimeSinceStartup;
		edges.Clear();
		List<int> tempEdges = TrisToEdges(triangles);
		await Task.Run(() =>
		{
			for (int i = 0; i < tempEdges.Count - 3; i += 3)
			{
				if (tempEdges[i] != -1)
				{
					int index = tempEdges[i];
					int[] edge = { tempEdges[i + 1], tempEdges[i + 2] };
					tempEdges[i] = -1;
					for (int j = i + 3; j < tempEdges.Count; j += 3)
					{
						if (tempEdges[j + 1] == edge[1] && tempEdges[j + 2] == edge[0])
						{
							edges.Add(index);
							edges.Add(tempEdges[j]);
							tempEdges[j] = -1;
							break;
						}
						if (j == tempEdges.Count - 3)
						{
							Debug.Log("Could not find mirror edge for [" + edge[0] + "," + edge[1] + "]");
						}
					}
				}
			}
		});
		Debug.Log("Edge Pairing: [" + numVertices + "] " + (Time.realtimeSinceStartup - startTime));
	}

	// Pair
	private async void PairEdgesFast()
	{
		float startTime = Time.realtimeSinceStartup;
		await Task.Run(() =>
		{
			Debug.Log("await task in pairedgesfast");
		});

		Debug.Log("Fast Edge Pairing: [" + numVertices + "] " + (Time.realtimeSinceStartup - startTime));
	}

	private GameObject BuildMesh(List<int> tris, string name)
	{
		GameObject meshObject = new GameObject(name);
		meshObject.transform.parent = transform;
		//meshObject.transform.position = 0.05f * vertices[tris[0]];

		var mesh = new Mesh
		{
			vertices = vertices.ToArray(),
			triangles = tris.ToArray()
		};
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		var meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
		meshRenderer.material.color = Random.ColorHSV(0, 1, 0.75f, 1, 0.5f, 1);
		meshRenderer.material.SetFloat("_Cull", (float)CullMode.Off);
		var meshFilter = meshObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;
		
		return meshObject;
	}

	private void BuildProjectedMesh()	// shows the mesh the way it is interpreted by the delaunator
	{
		projectedMesh = new GameObject("Projected Mesh");
		projectedMesh.transform.parent = transform;

		List<Vector3> pointsProjection = new List<Vector3>();
		foreach (Vector3 vertex in vertices)
		{
			Vector2 temp = StereographicProjection(vertex, true);
			pointsProjection.Add(new Vector3(temp.x, 1, temp.y));
		}

		var mesh = new Mesh
		{
			vertices = pointsProjection.ToArray(),
			triangles = delaunator.Triangles
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		var meshRenderer = projectedMesh.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = semiTransparent;
		var meshFilter = projectedMesh.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;
	}

	private void OopsAllBalls()
	{
		foreach (GameObject ball in myBals)
		{
			Destroy(ball);
		}
		myBals.Clear();

		for (int i = 0; i < vertices.Count; i++)
		{
			GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			ball.transform.position = vertices[i];
			ball.transform.localScale = 0.05f * Vector3.one;
			ball.GetComponent<MeshRenderer>().material.color = Color.white;
			ball.name = i.ToString();
			ball.transform.parent = transform;
			myBals.Add(ball);
		}

		//for (int i = 0; i < delaunator.Triangles.Length; i++)
		//{
		//	Debug.Log(i + ": " + delaunator.Triangles[i] + ", " + delaunator.Halfedges[i]);
		//}
	}

	private void SurfaceMovement()
	{
		foreach (GameObject center in triCenters)
		{
			Destroy(center);
		}
		triCenters.Clear();

		for (int i = 0; i < triangles.Count/3; i++)
		{
			GameObject center = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			center.transform.position = (vertices[triangles[3 * i]] + vertices[triangles[3 * i + 1]] + vertices[triangles[3 * i + 2]]) / 3;
			center.transform.localScale = 0.01f * Vector3.one;
			center.GetComponent<MeshRenderer>().material.color = Color.white;
			center.name = "Tri " + i.ToString();
			center.transform.parent = transform;
			triCenters.Add(center);

			LineRenderer lineRenderer = center.AddComponent<LineRenderer>();
			lineRenderer.startColor = Color.white;
			lineRenderer.endColor = Color.white;
			lineRenderer.widthMultiplier = 0.01f;
			lineRenderer.SetPositions(new Vector3[]{center.transform.position, center.transform.position * (PerlinNoise.Perlin3D(center.transform.position, 3) + 1)});
		}
	}
}