using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class V1_StellarSystemUI : MonoBehaviour
{
	void Start()
	{
		
	}

	void Update()
	{
		
	}

	public void NextScene()
	{
		SceneManager.LoadScene("V1_SCENE_AncestorSelection");
	}

	public void Randomize()
	{
		V1_StellarSystemController.instance.NewStellarSystem();
	}
}