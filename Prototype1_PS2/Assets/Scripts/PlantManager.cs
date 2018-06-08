using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlantManager : MonoBehaviour {

	public Moss mossPrefab;

	public List<Plant> plantsPrefabs;
	public List<Plant> crossedPlants;

	public List<PlantInformation> plantInformations;

	//public PlantSeed[] seedsPrefab;

	public List<Plant> plants;
	public List<PlantSeed> plantSeeds;

	private bool[] plantDiscovered;

	private int officialPlantNumber;

	public void InitializePlantInfoTab()
	{
		for (int i = 0; i < plantsPrefabs.Count; i++) {
			if (plantInformations.Count - 1 < i) {
				plantInformations.Add( new PlantInformation ());
			}
			plantInformations [i] = new PlantInformation (plantsPrefabs [i].name);
		}
	}

	public void DestroyPlants()
	{
		foreach (var item in plants) {
			Destroy (item.gameObject);
		}
		plants.Clear ();
		/*
		foreach (var item in plantSeeds) {
			Destroy (item.gameObject);
		}
		*/

	}


	public int AddPlantAndGetIndex(Plant plant)
	{
		plants.Add (plant);
		return plants.Count - 1;
	}


	public void Init()
	{
		plantDiscovered = new bool[plantsPrefabs.Count];
		for (int i = 0; i < plantDiscovered.Length; i++) {
			plantDiscovered [i] = true;
			plantsPrefabs [i].plantTypeIndex = i;

		}

		officialPlantNumber = plantsPrefabs.Count;

	}

	public void ResetLists()
	{
		crossedPlants.Clear ();
		plantsPrefabs.RemoveRange (officialPlantNumber, plantsPrefabs.Count - officialPlantNumber);
	}

	public void SerializePlants(WorldData wd)
	{
		wd.crossedPlants = new SerializedCrossedPlant[crossedPlants.Count];
		for (int i = 0; i < crossedPlants.Count; i++) {

			SerializedCrossedPlant scplant =
				new SerializedCrossedPlant (
				crossedPlants [i].parentIndex1,
				crossedPlants [i].parentIndex2,
				crossedPlants [i].name,
				crossedPlants [i].plantTextureIndex);
			wd.crossedPlants [i] = scplant;
		}

		wd.plants = new SerializedPlant[plants.Count];
		for (int i = 0; i < plants.Count; i++) {

			SerializedPlant sPlant = 
				new SerializedPlant (plants[i].name,
					plants[i].plantTypeIndex, 
					plants[i].time, 
					plants[i].transform.position, 
					plants[i].InitialDirection, 
					plants[i].fruitSequence,
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
