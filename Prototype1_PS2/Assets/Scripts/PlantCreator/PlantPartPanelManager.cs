using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPartPanelManager : MonoBehaviour {

	public Plant targetedPlant;
	public Plant targetedBranch;
	public Plant targetedSubBranch;
	public Leaf targetedLeaf;
	public Flower targetedFlower;
	public Fruit targetedFruit;


	[SerializeField] private ParameterListManager plant, leaf, flower, fruit;

	private GameObject lastSelected;

	public void TogglePlant()
	{
		lastSelected.SetActive (false);
		plant.gameObject.SetActive (true);
		lastSelected = plant.gameObject;
	}

	public void ToggleLeaf()
	{
		lastSelected.SetActive (false);
		leaf.gameObject.SetActive (true);
		lastSelected = leaf.gameObject;
	}


	public void Initialize()
	{

		plant.Initialize (this);
		leaf.Initialize (this);
		flower.Initialize (this);
		fruit.Initialize (this);

		lastSelected = plant.gameObject;

		plant.gameObject.SetActive (true);
		leaf.gameObject.SetActive (false);
		flower.gameObject.SetActive (false);
		fruit.gameObject.SetActive (false);

	}

	void Start () 
	{
		Initialize ();
	}
}
