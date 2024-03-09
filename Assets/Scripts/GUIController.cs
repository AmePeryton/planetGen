using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{

	[Header ("System Control Panel")]
	public GameObject systemPanel;
	public StellarSystem systemScript;
	public float systemPanelSmoothing;

	private bool showSystemPanel;
	private Vector3 systemPanelPos;
	private RectTransform systemPanelRect;

	[Header("Star Panel")]
	public GameObject starPanel;
	public GameObject targetStar;
	private bool showStarPanel;

	[Header("Planet Panel")]
	public GameObject planetPanel;
	public GameObject targetPlanet;
	private bool showPlanetPanel;

	void Start()
	{
		GUIReset();
	}

	void Update()
	{
		systemPanelRect.anchoredPosition = Vector3.Lerp(systemPanelRect.anchoredPosition, systemPanelPos, systemPanelSmoothing);


	}

	public void GUIReset()
	{
		showSystemPanel = false;
		systemPanelPos = new Vector3(200, 0, 0);
		systemPanelRect = systemPanel.GetComponent<RectTransform>();

		showStarPanel = true;
		ToggleStarPanel(null);
		showPlanetPanel = true;
		TogglePlanetPanel(null);
	}

	public void ToggleSystemPanel()
	{
		showSystemPanel = !showSystemPanel;
		if (showSystemPanel)
		{
			systemPanelPos = new Vector3(0, 0, 0);
		}
		else
		{
			systemPanelPos = new Vector3(200, 0, 0);
		}
	}

	public void ToggleStarPanel(GameObject caller)
	{
		showStarPanel = !showStarPanel;

		if (showStarPanel)
		{
			if (showPlanetPanel)
			{
				TogglePlanetPanel(null);
			}

			targetStar = caller;
			starPanel.GetComponent<StarPanel>().starScript = targetStar.GetComponent<StarProperties>();
			starPanel.SetActive(true);
		}
		else
		{
			targetStar = null;
			starPanel.SetActive(false);
		}
	}

	public void TogglePlanetPanel(GameObject caller)
	{
		showPlanetPanel = !showPlanetPanel;

		if(showPlanetPanel)
		{
			if(showStarPanel)
			{
				ToggleStarPanel(null);
			}

			targetPlanet = caller;
			planetPanel.GetComponent<PlanetPanel>().planetScript = targetPlanet.GetComponent<PlanetProperties>(); ;
			planetPanel.SetActive(true);
		}
		else
		{
			targetPlanet = null;
			planetPanel.SetActive(false);
		}
	}
}
