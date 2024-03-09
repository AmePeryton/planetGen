using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;
using Random = UnityEngine.Random;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using System.Linq;

public class MappedPlanet : MonoBehaviour
{
	public bool rotate;
	[Range(0f, 0.1f)]
	public float rotationSpeed = 0.01f;
	public bool perlin = true;
	public bool colored = false;
	[Header("Perlin Noise Generation")]
	[Range(0, 10)]
	public float frequency = 1;
	[Header("Export")]
	public Color[] mapColors = {
		new Color(0f, 0f, 0f),
		new Color(1f, 1f, 1f)
	};
	public int resolution = 1024;	// height of the map
	public string fileName = "map";
	public bool export = false;
	[Header("Tectonics")]
	public bool tectonics = false;
	public int numPlates;
	public int numVertices;
	[Range(0f, 1f)]
	public float jitter;
	public int seed;
	public bool rotationTime;
	public List<TectonicPlate> plates = new List<TectonicPlate>();
	public List<TectonicBoundary> boundaries = new List<TectonicBoundary>();
	public Color[] plateColors = {
		new Color(0.1f, 0.15f, 0.3f),
		new Color(0.15f, 0.3f, 0.15f)
	};
	public enum BoundaryType { Unknown, Divergent, Convergent, Transform };

	[Header("Triangulation Data")]
	public List<Vector3> vertices = new List<Vector3>();
	public List<int> triangles = new List<int>();
	private List<FillableVertex> fillableVertices = new List<FillableVertex>();
	private List<PlateFiller> fillers = new List<PlateFiller>();
	private List<GameObject> plateMeshes = new List<GameObject>();

	[Header("Crust Simulation")]
	public bool simulateCrust;
	public float threshold;
	public CrustSim crustSim;

	private MappedPlanetUI UIController;
	private Texture2D map;
	private Renderer myRenderer;
	private RawImage UImap;

	public class FillableVertex
	{
		public List<FillableVertex> edges;
		public List<PlateFiller> plates;

		public FillableVertex()
		{
			this.edges = new List<FillableVertex>();
			this.plates = new List<PlateFiller>();
		}
	}

	public class PlateFiller
	{
		public List<FillableVertex> vertices;
		public List<FillableVertex> expandableVertices;

		public PlateFiller(FillableVertex first)
		{
			this.vertices = new List<FillableVertex>() { first };
			this.expandableVertices = new List<FillableVertex>() { first };
		}
	}

	[Serializable]
	public class TectonicPlate	// collections of cells defining a tectonic plate
	{
		public List<Vector3> vertices;
		public List<int> triangles;
		public Color color;
		public GameObject mesh;
		public Vector3 primaryPosition;
		public float velocityScalar;
		public Vector3 velocityVector;

		public TectonicPlate(List<Vector3> vertices, List<int> triangles, Color color)
		{
			this.vertices = vertices;
			this.triangles = triangles;
			this.color = color;
			this.mesh = null;
			this.primaryPosition = vertices[0];
			this.velocityScalar = 0;
			this.velocityVector = Vector3.zero;
		}
	}

	[Serializable]
	public class CrustSim
	{
		public float threshold;
		public List<Vector3> vertices;
		public List<int> triangles;
		public List<int> edges;		// two triangles that share an edge, listed by start index of each tri in pairs
		public List<Vector3> velocities;	// velocity at the average point of each triangle
		public GameObject mesh;

		public CrustSim(float threshold, List<Vector3> vertices, List<int> triangles, List<int> edges, List<Vector3> velocities)
		{
			this.threshold = threshold;
			this.vertices = vertices;
			this.triangles = triangles;
			this.edges = edges;
			this.velocities = velocities;
			this.mesh = null;
		}
	}

	public class CrustSegment
	{
		public Vector3[] vertices;	// position of the 3 vertices of this triangle
		public Vector3 velocity;	// velocity sampled from the average position of the vertices
		public CrustSegment[] neighborSegments;		// 3 neighboring triangles (edge sharers)

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

	public class TectonicBoundary
	{
		public List<Vector3> vertices;
		public BoundaryType boundaryType;

		public TectonicBoundary()
		{
			this.vertices = new List<Vector3>();
			this.boundaryType = BoundaryType.Unknown;
		}
	}

	public class VertexMovementVector
	{
		public float rawValue;
	}

	void Start()
	{
		UIController = GetComponentInChildren<MappedPlanetUI>();
		myRenderer = GetComponent<Renderer>();
		UImap = GetComponentInChildren<RawImage>();
		map = new Texture2D(2 * resolution, resolution);
	}

	void Update()
	{
		if (perlin)
		{
			perlin = false;
			GenerateMap();
		}

		if (export)
		{
			export = false;
			ExportMap();
		}

		if (rotate)
		{
			transform.Rotate(0, -rotationSpeed, 0);
		}

		if (tectonics)	// generate a bunch of separate plates
		{
			tectonics = false;
			GenerateVertices();
			Triangulate();
			PlaceFillableVertices();
			GenerateTectonicPlates();
			PlacePlates();
		}

		if (rotationTime)
		{
			rotationTime = false;
			RandomlyRotatePlates();
		}

		if (simulateCrust)
		{
			simulateCrust = false;
			GenerateVertices();
			Triangulate();
			//SimulateCrust();
			BuildCrustSegments();
		}
	}

	public void RandomlyRotatePlates()
	{
		foreach (TectonicPlate plate in plates)
		{
			plate.mesh.transform.rotation = Quaternion.Lerp(plate.mesh.transform.rotation, Random.rotation, 0.01f);
		}
	}

	public float NoiseFunction(Vector3 input)
	{
		float output = LayeredNoise(input);
		return output;
	}

	public float LayeredNoise(Vector3 input)
	{
		//float output = 2f / 3f * PerlinNoise.Perlin3D(input, frequency) + 1f / 3f * PerlinNoise.Perlin3D(input, 2 * frequency);
		float output = PerlinNoise.Perlin3D(input, frequency);
		output = (output + 1f) / 2f;
		return output;
	}

	public async void GenerateMap()
	{
		float startTime = Time.realtimeSinceStartup;
		map = new Texture2D(2 * resolution, resolution);
		Color[] colors = new Color[2 * resolution * resolution];
		myRenderer.material.mainTexture = map;

		await Task.Run(() =>
		{
			for (int i = 0; i < resolution; i++)
			{
				for (int j = 0; j < 2 * resolution; j++)
				{
					Vector2 UV = new Vector2(
						(360f / (2f * resolution - 1)) * j - 180,
						(180f / (resolution - 1)) * i - 90);
					Vector3 realPosition = new Vector3(
						-Mathf.Sin(Mathf.Deg2Rad * UV.x) * Mathf.Cos(Mathf.Deg2Rad * UV.y),
						Mathf.Sin(Mathf.Deg2Rad * UV.y),
						Mathf.Cos(Mathf.Deg2Rad * UV.x) * Mathf.Cos(Mathf.Deg2Rad * UV.y)
						);

					float height = NoiseFunction(realPosition);

					if (colored)
					{
						colors[j + i * 2 * resolution] = ValueToColor(height);
					}
					else
					{
						colors[j + i * 2 * resolution] = Color.Lerp(mapColors[0], mapColors[1], height);
					}
				}
			}
		});

		map.SetPixels(colors);
		map.Apply();

		myRenderer.material.mainTexture = map;
		UImap.texture = map;
		Debug.Log("Map: " + (Time.realtimeSinceStartup - startTime));
	}

	public void ExportMap()
	{
		byte[] bytes = map.EncodeToPNG();
		string saveLocation = "C:\\Users\\ameli\\Desktop\\maps\\" + fileName + ".png";
		File.WriteAllBytes(saveLocation, bytes);
		Debug.Log("Saved to " + saveLocation);
	}

	private Vector3 PixeltoPosition(int x, int y)
	{
		Vector2 UV = new Vector2(
			(360f / (2f * resolution - 1)) * x,
			(180f / (resolution - 1)) * y - 90);
		Vector3 realPosition = new Vector3(
			-Mathf.Sin(Mathf.Deg2Rad * UV.x) * Mathf.Cos(Mathf.Deg2Rad * UV.y),
			Mathf.Sin(Mathf.Deg2Rad * UV.y),
			Mathf.Cos(Mathf.Deg2Rad * UV.x) * Mathf.Cos(Mathf.Deg2Rad * UV.y)
			);
		return realPosition;
	}

	/* 
	NOTE: "Generate" means the function creates new data, "Place" means it organizes existing data

	Step 1: generate list of points on sphere surface (only need the vector3)
	Step 2: Triangulate points and store triangles as list of indexes of the vertices
	Step 3: Populate fillable vertices with edges based on triangulation
	Step 4: Fill vertices with plates (breadth first)
	Step 5: Generate list of TectonicVertices and Tectonic plates based on this data
	Step 6: Fill Octree?
	Step 7: Draw to map
	
	vertex types:
		- unclaimed
			make expandable
		- claimed by self and expandable
			do nothing
		- claimed by self and interior
			do nothing
		- claimed by self and a border of another (claimed by 2 or more)
			do nothing
		- claimed by other and expandable
			make border to both plates (non expandable)
		- claimed by other and interior
			shouldn't be possible
		- claimed by other and a border (not to self)
			make border to self and the others
	
	select some random cells
	create a new NewTectonicPlate for each of them, with only the coresponding cell in its expandables and vertices lists
	for each plate, in order:
		get the neighbors of each expandable node belonging to the plate,
			if claimed by self, do nothing
			else if claimed by other, add to vertices AND remove from other plate's expandables
			else, add to vertices and expandables
			finally, remove this expandable node from expandables list
	if not all cells are claimed, do loop again
	*/

	public void GenerateVertices()	// fills the vertices list with specified number of vector3s on the sphere
	{
		Random.InitState(seed);
#pragma warning disable CS0618
		seed = Random.seed;
#pragma warning restore CS0618

		float maxJitter = Mathf.Sqrt(2) * Mathf.Pow(Mathf.PI, -Mathf.Log10(numVertices)); // sqrt(2) * pi^(-log_10(n))

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

	public void Triangulate()	// calculate delaunay triangulation of the points, saved as list of indexes (triangles)
	{
		float startTime = Time.realtimeSinceStartup;

		triangles.Clear();
		List<Vector2> pointsProjection = new List<Vector2>();
		foreach (Vector3 vertex in vertices)
		{
			pointsProjection.Add(StereographicProjection(vertex, true));
		}
		Delaunator delaunator = new Delaunator(pointsProjection.ToPoints());	// main delaunator

		pointsProjection.Clear();
		foreach (int point in delaunator.Hull)
		{
			pointsProjection.Add(StereographicProjection(vertices[point], false));
		}
		Delaunator hullDelaunator = new Delaunator(pointsProjection.ToPoints());	// triangulate hole at south pole

		triangles = delaunator.Triangles.ToList<int>();
		foreach (int tri in hullDelaunator.Triangles)
		{
			triangles.Add(delaunator.Hull[tri]);	// combine main triangles and hole correction triangles
		}

		Debug.Log("Triangulation: " + (Time.realtimeSinceStartup - startTime));
	}

	public void PlaceFillableVertices()	// create fillable vertices corresponding to vertices, and populate with their edges
	{
		fillableVertices.Clear();
		foreach (Vector3 vertex in vertices)
		{
			fillableVertices.Add(new FillableVertex());
		}

		for (int i = 0; i < triangles.Count; i += 3)	// add edges to fillable vertex objects
		{
			FillableVertex a = fillableVertices[triangles[i]];
			FillableVertex b = fillableVertices[triangles[i + 1]];
			FillableVertex c = fillableVertices[triangles[i + 2]];

			if (!a.edges.Contains(b)) { a.edges.Add(b); }
			if (!a.edges.Contains(c)) { a.edges.Add(c); }
			if (!b.edges.Contains(c)) { b.edges.Add(c); }
			if (!b.edges.Contains(a)) { b.edges.Add(a); }
			if (!c.edges.Contains(a)) { c.edges.Add(a); }
			if (!c.edges.Contains(b)) { c.edges.Add(b); }
		}

	}

	public void GenerateTectonicPlates()	// create some tectonic plates starting at random vertices and expand to fill
	{
		fillers.Clear();
		for (int i = 0; i < plateMeshes.Count; i++)
		{
			Destroy(plateMeshes[i]);
		}
		plateMeshes.Clear();

		// NOTE: change algorithm so plates cant start with a common edge
		for (int i = 0; i < numPlates; i++)
		{
			int index = Random.Range(0, numVertices);
			if (fillableVertices[index].plates.Count == 0)
			{
				PlateFiller newPlateFiller = new PlateFiller(fillableVertices[index]);
				ExpandPlate(newPlateFiller);
				fillers.Add(newPlateFiller);
			}
			else
			{
				i--;
			}
		}

		int numExpandables;
		do
		{
			numExpandables = 0;
			foreach (PlateFiller plateFiller in fillers)
			{
				ExpandPlate(plateFiller);
				numExpandables += plateFiller.expandableVertices.Count;
			}
		} while (numExpandables > 0);

		// fix holes often left by intersection of 3 or more plates
		for (int i = 0; i < triangles.Count; i += 3)
		{
			List<FillableVertex> verts = new List<FillableVertex>() {
				fillableVertices[triangles[i]],
				fillableVertices[triangles[i+1]],
				fillableVertices[triangles[i+2]]
			};
			List<PlateFiller> common = verts[0].plates.Intersect(verts[1].plates).Intersect(verts[2].plates).ToList<PlateFiller>();

			if (common.Count == 0)
			{
				float[] edgeLengths = new float[] {
					Vector3.Distance(vertices[triangles[i+1]], vertices[triangles[i+2]]),	// edge opposite verts[0]
					Vector3.Distance(vertices[triangles[i+2]], vertices[triangles[i]]),		// edge opposite verts[1]
					Vector3.Distance(vertices[triangles[i]], vertices[triangles[i+1]])		// edge opposite verts[2]
				};

				for (int j = 0; j < 3; j++)
				{
					int maxIndex = Array.IndexOf(edgeLengths, edgeLengths.Max());
					List<PlateFiller> claimers = verts[(maxIndex + 1) % 3].plates.Intersect(verts[(maxIndex + 2) % 3].plates).ToList<PlateFiller>();
					if (claimers.Count > 0)
					{
						claimers[0].vertices.Add(verts[maxIndex]);
						verts[maxIndex].plates.Add(claimers[0]);
						break;	// if this vertex has a plate that claims both other vertices, claim and break
					}
					else
					{
						Debug.Log("Claimer edge case: no common plates claim long edge.");
						edgeLengths[maxIndex] = 0;	// make previous max value 0, so it goes with the next max next loop
					}
				}
			}
		}
		Debug.Log("end");
	}

	public void ExpandPlate(PlateFiller plate)	// single expansion at a time
	{
		List<FillableVertex> newFrontier = new List<FillableVertex>();
		foreach (FillableVertex frontier in plate.expandableVertices)
		{
			foreach (FillableVertex edge in frontier.edges)
			{
				if (edge.plates.Contains(plate))	// if claimed by self, do nothing
				{
				}
				else if (edge.plates.Count == 0)	// if unclaimed, claim and add to vertices and expandables
				{
					edge.plates.Add(plate);
					plate.vertices.Add(edge);
					newFrontier.Add(edge);
				}
				else	// if claimed by others but not self, remove from their expandables, claim, and add to vertices
				{
					foreach (PlateFiller otherPlate in edge.plates)
					{
						otherPlate.expandableVertices.Remove(edge);
					}

					edge.plates.Add(plate);
					plate.vertices.Add(edge);
				}
			}
			
		}
		plate.expandableVertices = newFrontier;
	}

	public async void PlacePlates()
	{
		float startTime = Time.realtimeSinceStartup;
		plates.Clear();
		foreach (PlateFiller filler in fillers)
		{
			List<Vector3> verts = new List<Vector3>();
			List<int> tris = new List<int>();
			await Task.Run(() =>	// a lot of loops, should be async
			{
				List<int> vertIndices = new List<int>();
				foreach (FillableVertex vert in filler.vertices)
				{
					verts.Add(vertices[fillableVertices.IndexOf(vert)]);
					vertIndices.Add(fillableVertices.IndexOf(vert));
				}

				for (int i = 0; i < triangles.Count; i += 3)
				{
					if (vertIndices.Contains(triangles[i]) && vertIndices.Contains(triangles[i + 1]) && vertIndices.Contains(triangles[i + 2]))
					{
						tris.Add(triangles[i]);
						tris.Add(triangles[i + 1]);
						tris.Add(triangles[i + 2]);
					}
				}
			});

			Color plateColor;
			//plateColor = plateColors[Random.Range(0, 2)] * Random.Range(0.7f, 1f);
			plateColor = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);

			TectonicPlate newPlate = new TectonicPlate(verts, tris, plateColor);
			plates.Add(newPlate);
			TectonicPlateMesh(newPlate);
		}
		Debug.Log("Place Plates Async: " + (Time.realtimeSinceStartup - startTime));
	}

	public void TectonicPlateMesh(TectonicPlate plate)
	{
		var mesh = new Mesh
		{
			vertices = vertices.ToArray(),
			triangles = plate.triangles.ToArray()
		};
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		plate.mesh = new GameObject("Plate " + plates.IndexOf(plate) + " Mesh");
		plate.mesh.transform.parent = transform;
		var meshRenderer = plate.mesh.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
		meshRenderer.material.color = plate.color;
		meshRenderer.material.SetFloat("_Cull", (float)CullMode.Off);
		var meshFilter = plate.mesh.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;
		plate.mesh.transform.position = 0.01f * plate.primaryPosition;
		plateMeshes.Add(plate.mesh);
		//DisplayBall(plate.primaryPosition);
	}

	public void DisplayBall(Vector3 position)
	{
		GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		ball.transform.position = position;
		ball.transform.localScale = 0.05f * Vector3.one;
	}

	public Vector2 StereographicProjection(Vector3 position, bool north)
	{
		if (north)
		{
			return new Vector2(position.x / (1 + position.y), position.z / (1 + position.y));
		}
		return new Vector2(-position.x / (1 - position.y), position.z / (1 - position.y));
	}

	public Color ValueToColor(float value)
	{
		if (value < 0) { return Color.black; }
		if (value <= 0.25f) { return new Color(0, 4 * value, 1); }
		if (value <= 0.5f) { return new Color(0, 1, 2 - 4 * value); }
		if (value <= 0.75f) { return new Color(4 * value - 2, 1, 0); }
		if (value <= 1) { return new Color(1, 4 - 4 * value, 0); }
		return Color.white;
	}

	public void SimulateCrust()
	{
		crustSim = new CrustSim(threshold, vertices, triangles, null, null);
		var mesh = new Mesh
		{
			vertices = vertices.ToArray(),
			triangles = triangles.ToArray()
		};
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		crustSim.mesh = new GameObject("Crust");
		crustSim.mesh.transform.parent = transform;
		var meshRenderer = crustSim.mesh.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
		meshRenderer.material.color = Color.blue;
		meshRenderer.material.SetFloat("_Cull", (float)CullMode.Off);
		var meshFilter = crustSim.mesh.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;
	}

	public void BuildCrustSegments()
	{
		Triangulate();
		List<CrustSegment> segments = new List<CrustSegment>();
		for (int i = 0; i < triangles.Count; i += 3)
		{
			Vector3[] verts = new Vector3[] { };
			//float velocity = 0;
			//CrustSegment newSegment = new CrustSegment();

		}
	}
}