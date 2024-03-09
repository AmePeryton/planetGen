using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class V1_SaveData
{
	// Standard Data
	public string name;
	public string dateCreated;
	public string dateModified;
	public GameState gameState;

	// Stellar Object Data
	public StarData starData;
	public PlanetData planetData;
	
	public enum GameState
	{
		Menu = 0,			// Game saved while in main menu, before stellar system loaded
		Stellar,			// Game saved while tweaking the Stellar System or the Planet
		LCA,				// Game saved while choosing a LCA
		CreatureCreator,	// Game saved while tweaking your organism (who up tweaking they organism) or in physical simulaton
		Gameplay			// Game saved during open world gameplay
	}

	public V1_SaveData()
	{
		name = "DEFAULT NAME";
		dateCreated = "1970-01-01";
		dateModified = "1970-01-01";
		gameState = GameState.Menu;
		starData = new StarData();
		planetData = new PlanetData();
	}
}
[System.Serializable]
public class StarData
{
	public float mass;
	public float age;

	public StarData()
	{
		mass = 0;
		age = 0;
	}
}

[System.Serializable]
public class PlanetData
{
	public float mass;
	public float radius;
	public float distance;

	public PlanetData()
	{
		mass = 0;
		radius = 0;
		distance = 0;
	}
}