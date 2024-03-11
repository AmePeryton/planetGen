using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_TitlePanelController : MonoBehaviour
{
	public void ButtonQuit()
	{
		Debug.Log("Quit Button Pressed");
		Application.Quit();
	}
}