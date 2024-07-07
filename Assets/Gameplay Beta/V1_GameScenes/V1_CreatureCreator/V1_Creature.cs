using System;
using System.Collections.Generic;
using UnityEngine;

public class V1_Creature : MonoBehaviour
{
	public CurrencyBalance currencyBalance;
	public HashSet<V1_OrganNode> organNodes;
	public int organNum;

	[Header("Prefabs")]
	public GameObject organNodePrefab;

	private void Awake()
	{
		organNodes = new HashSet<V1_OrganNode>();
	}

	void Start()
	{
		for (int i = 0; i < organNum; i++)
		{
			AddOrganNode();
		}
	}

	[ContextMenu("Calculate Balance")]
	public void CalculateBalance()
	{
		currencyBalance = new CurrencyBalance();

		foreach (V1_OrganNode node in organNodes)
		{
			currencyBalance.energy += node.type.outputs.energy - node.type.intakes.energy;
			currencyBalance.calories += node.type.outputs.calories - node.type.intakes.calories;
			currencyBalance.minerals += node.type.outputs.minerals - node.type.intakes.minerals;
			currencyBalance.light += node.type.outputs.light - node.type.intakes.light;
			currencyBalance.oxygen += node.type.outputs.oxygen - node.type.intakes.oxygen;
			currencyBalance.co2 += node.type.outputs.co2 - node.type.intakes.co2;
			currencyBalance.neuron += node.type.outputs.neuron - node.type.intakes.neuron;
		}
	}

	[ContextMenu("Add Organ Node")]
	public void AddOrganNode()
	{
		V1_OrganNode newOrganNode = Instantiate(organNodePrefab).GetComponent<V1_OrganNode>();
		newOrganNode.position = UnityEngine.Random.insideUnitSphere;
		organNodes.Add(newOrganNode);
	}
}

[Serializable]
public enum SymmetryType
{
	Asymmetrical,
	Bilateral,
	Radial
}

[Serializable]
public struct CurrencyBalance
{
	public float energy;
	public float calories;
	public float minerals;
	public float light;
	public float oxygen;
	public float co2;
	public float neuron;
}