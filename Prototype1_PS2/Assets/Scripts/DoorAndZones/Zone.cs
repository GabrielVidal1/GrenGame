using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {



	public int NbOfPlants {
		get { return plantIndexesInPlantArray.Count; }
	}


	public int nbOfPoints;

	public List<int> plantIndexesInPlantArray;


	void Start () 
	{
		
	}
	
	// Update is called once per frame
	public int TotalPoints()
	{
		int sum = 0;
		foreach (int index in plantIndexesInPlantArray) {
			sum += GameManager.gm.pm.plants[index].pointValue;
		}
		return sum;
	}


	public void AddPlant(int plantIndexInPlantArray)
	{
		plantIndexesInPlantArray.Add (plantIndexInPlantArray);
	}
}
