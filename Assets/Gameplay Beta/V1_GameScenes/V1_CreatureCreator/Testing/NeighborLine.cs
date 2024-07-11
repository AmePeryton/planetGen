using UnityEngine;

public class NeighborLine : MonoBehaviour
{
	public LineRenderer lineRenderer;
	public GameObject[] points;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		points = new GameObject[2];
	}

	void Update()
	{
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		lineRenderer.SetPosition(0, points[0].transform.position);
		lineRenderer.SetPosition(1, points[1].transform.position);
	}
}