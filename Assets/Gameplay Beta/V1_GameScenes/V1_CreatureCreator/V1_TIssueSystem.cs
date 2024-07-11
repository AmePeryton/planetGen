using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_TIssueSystem : MonoBehaviour
{
	void Start()
	{
		
	}

	void Update()
	{
		
	}
}

[Serializable]
public class TissueType
{
	public string name;
	public float[] nutrientBalance; // rate of intake/output of nutrients
	public Color color;

	/* Potential tissue values:
		* Density
		* Cell size
		* Cell wall strength
		* Division speed
		* Lifetime
		* Color
	*/

	/* Potential tissue capabilities:
		* Store substances
		* Secrete substances
		* Transport substances
		* Absorb substances
		* Produce pigments (stays in tissue)
		* Photosynthesize
		* Contract and expand (ie. muscles)
		* Die on maturity
	*/

	public TissueType(string t)
	{
		name = t;
		nutrientBalance = new float[]
		{
			0,	// oxygen
			0,	// water
			0,	// CO2
			0,	// carbs / sugars
			0,	// protein
			0,	// fat
			0,	// minerals
		};

		color = UnityEngine.Random.ColorHSV();
	}
}

[Serializable]
public class Tissue: ITissueNeighbor
{
	public string name;
	public TissueType tissueType;
	public float[] nutrientStorage;	// oxygen, water, CO2, carbs/sugars, protein, fat, minerals
	public List<ITissueNeighbor> neighbors;

	/* Neighbors can be
		* Other tissues
		* Cavities
		* Parts of the environment
	*/

	/* Tissues can be neighbors with any other tissue in the body, regardless of organ
	*/

	public Tissue(string n, TissueType t)
	{
		name = n;
		tissueType = t;
		nutrientStorage = new float[]
		{
			0,	// oxygen
			0,	// water
			0,	// CO2
			0,	// carbs / sugars
			0,	// protein
			0,	// fat
			0,	// minerals
		};
		neighbors = new List<ITissueNeighbor>();
	}

	public void AddNeighbor(ITissueNeighbor newNeighbor)
	{
		if (newNeighbor != this)
		{
			neighbors.Add(newNeighbor);
		}
	}
}

// Unused for now
//[Serializable]
//public class Organ
//{
//	public float name;
//	public Vector3 position;
//	public HashSet<Tissue> tissues;

//	public Organ()
//	{
//		tissues = new HashSet<Tissue>();
//	}
//}

[Serializable]
public class Cavity: ITissueNeighbor
{
	public string name;
	public float[] nutrientStorage; // oxygen, water, CO2, carbs/sugars, protein, fat, minerals
	public List<ITissueNeighbor> neighbors;

	public Cavity(string n)
	{
		name = n;
		nutrientStorage = new float[]
		{
			0,	// oxygen
			0,	// water
			0,	// CO2
			0,	// carbs / sugars
			0,	// protein
			0,	// fat
			0,	// minerals
		};
		neighbors = new List<ITissueNeighbor>();
	}

	public void AddNeighbor(ITissueNeighbor newNeighbor)
	{
		if (newNeighbor != this)
		{
			neighbors.Add(newNeighbor);
		}
	}
}

[Serializable]
public class Environment : ITissueNeighbor
{
	public string name;
	public float[] nutrientStorage; // oxygen, water, CO2, carbs/sugars, protein, fat, minerals
	public List<ITissueNeighbor> neighbors;

	public Environment(string n)
	{
		name = n;
		nutrientStorage = new float[]
		{
			0,	// oxygen
			0,	// water
			0,	// CO2
			0,	// carbs / sugars
			0,	// protein
			0,	// fat
			0,	// minerals
		};
		neighbors = new List<ITissueNeighbor>();
	}

	public void AddNeighbor(ITissueNeighbor newNeighbor)
	{
		if (newNeighbor != this)
		{
			neighbors.Add(newNeighbor);
		}
	}
}

public interface ITissueNeighbor
{
	//public float GiveNutrient();
	//public float TakeNutrient();
	public void AddNeighbor(ITissueNeighbor newNeighbor);
}