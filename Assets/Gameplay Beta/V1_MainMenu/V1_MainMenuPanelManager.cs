using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_MainMenuPanelManager : MonoBehaviour
{
	public static V1_MainMenuPanelManager instance { get; private set; }
	public ActivePanel activePanel;
	public List<GameObject> panels; // Title, Settings, Play

	[System.Serializable]
	public enum ActivePanel
	{
		Title,
		Settings,
		Play
	}

	private void Awake()
	{
		SwitchPanel(0);
	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	public void SwitchPanel(int panel)
	{
		activePanel = (ActivePanel)panel;
		foreach (GameObject p in panels)
		{
			p.SetActive(false);
		}
		panels[panel].SetActive(true);
	}
}