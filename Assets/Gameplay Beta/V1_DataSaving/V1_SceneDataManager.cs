using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_SceneDataManager : MonoBehaviour
{
	//public static V1_SceneDataManager instance { get; private set; }
	public GameState gameState;

	public virtual void LoadSceneData()
	{

	}
	
	public virtual void SaveSceneData()
	{
		Debug.Log("PARENT FUNCTION CALLED!!!");
	}
}