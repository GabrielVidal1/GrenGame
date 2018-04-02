using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlantManager : MonoBehaviour {

	public Plant[] plantsPrefabs;
	public PlantSeed seedsPrefab;

	public List<Plant> plants;
	public List<PlantSeed> plantSeeds;

	private bool[] plantDiscovered;

	void Start()
	{
		plantDiscovered = new bool[plantsPrefabs.Length];
		for (int i = 0; i < plantDiscovered.Length; i++) {
			plantDiscovered [i] = true;
			plantsPrefabs [i].indexInPlantManagerArray = i;

		}

	}

	public void SerializePlants(WorldData wd)
	{
		wd.plants = new SerializedPlant[plants.Count];



		for (int i = 0; i < plants.Count; i++) {



			SerializedPlant sPlant = 
				new SerializedPlant (plants[i].indexInPlantManagerArray, 
					plants[i].time, 
					plants[i].transform.position, 
					plants[i].initialDirection, 
					plants[i].plantSeed, 
					i);
			wd.plants [i] = sPlant;
		}
	}

	public void SerializeSeeds(WorldData wd)
	{
		wd.seeds = new SerializedPlantSeed[plantSeeds.Count];



		for (int i = 0; i < plantSeeds.Count; i++) {



			SerializedPlantSeed seed = 
				new SerializedPlantSeed (plantSeeds [i].indexInPlantManager, 
					plantSeeds [i].transform.position,
					plantSeeds[i].gameObject.activeSelf);
			wd.seeds [i] = seed;

		}
	}


	public string GetPlantNameFromIndex(int index)
	{
		if (plantDiscovered [index]) 
		{
			return plantsPrefabs [index].name;
		}
		throw new UnityException("plant hasn't been discovered yet");
	}

	public bool isPlantDiscoveredFromIndex(int index)
	{
		return plantDiscovered [index];
	}

}
