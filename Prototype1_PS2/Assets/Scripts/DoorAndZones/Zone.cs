using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

	public int nbOfPoints;

	public List<int> plantIndexesInPlantArray;

	private List<Door> doors;

	void Start () 
	{
		
	}

	public void AddToDoorArray(Door door)
	{
		if (doors == null)
			doors = new List<Door> ();

		doors.Add (door);
	}

	// Update is called once per frame
	public int TotalPoints()
	{
		int sum = 0;
		foreach (int index in plantIndexesInPlantArray) {
			sum += GameManager.gm.pm.plants[index].pointValue;
		}
		return sum + nbOfPoints;
	}


	public void AddPlant(int plantIndexInPlantArray)
	{
		plantIndexesInPlantArray.Add (plantIndexInPlantArray);

		foreach (Door door in doors) {
			door.UpdateSlider ();
			Debug.Log ("tteste");
		}
	}
}
