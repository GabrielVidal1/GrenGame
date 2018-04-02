using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class WorldSerialization : MonoBehaviour{

	public WorldData worldData;

	public void SaveWorldFile()
	{

		//SAVING THE FILE
		string path = Application.persistentDataPath + "/" + name.ToUpper () + ".grenworld";

		Debug.Log (path);
		BinaryFormatter bf = new BinaryFormatter ();


		FileStream file = File.Create( path );

		bf.Serialize( file, worldData );
		file.Close();
	}

	public void LoadWorldFile()
	{
		if (File.Exists(Application.persistentDataPath + "/" + name.ToUpper () + ".grenworld")) {

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + name.ToUpper () + ".grenworld", FileMode.Open);

			worldData = (WorldData)bf.Deserialize (file);
			file.Close ();

		} else {
			worldData = new WorldData();
		}

	}

	public void SerializeWorld()
	{

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		List<string> playersName = new List<string> ();
		List<Vector3> playerPositions = new List<Vector3> ();
		List<SerializedPlayerInventory> playerInventories = new List<SerializedPlayerInventory> ();

		foreach (var p in players) {

			Player player = p.GetComponent<Player> ();

			int index = -1;
			for (int i = 0; i < playersName.Count; i++) {
				if (player.playerName == playersName [i]) {
					index = i;
					playerPositions [i] = player.transform.position;

					playerInventories [i] = new SerializedPlayerInventory (player.GetComponent<PlayerInventory> ());
				}
			}
			if (index == -1) {
				playersName.Add (player.playerName);
				playerPositions.Add(player.transform.position);
				playerInventories.Add(new SerializedPlayerInventory (player.GetComponent<PlayerInventory> ()));

			}
		}

		worldData.playersName = playersName.ToArray ();
		worldData.playerPositions = playerPositions.ToArray ();
		worldData.playerInventories = playerInventories.ToArray ();


		GameManager.gm.pm.SerializePlants (worldData);
		Debug.Log(worldData.plants.Length);


		GameManager.gm.pm.SerializeSeeds (worldData);
		Debug.Log(worldData.seeds.Length);


		(new GameObject ()).AddComponent<Test> ().wd = worldData;

	}

	public void DeserializeWorld(WorldData worldData)
	{

		CanvasManager.cm.multiplayerMenu.multiplayerClientLoadingPlants.SetActive (true);

		//PLANTS
		Debug.Log("nb of seeds : " + worldData.plants.Length);

		foreach (SerializedPlant serializedPlant in worldData.plants) {
			
			Plant plant = Instantiate (GameManager.gm.pm.plantsPrefabs [serializedPlant.plantTypeIndex], serializedPlant.initialPosition, Quaternion.identity).GetComponent<Plant> ();
			plant.initialDirection = serializedPlant.initialDirection;
			plant.SetSeed (serializedPlant.seed);
			plant.time = serializedPlant.plantTime;
			plant.indexInGameData = serializedPlant.indexInGameData;

			GameManager.gm.pm.plants.Add (plant);

			plant.InitializePlant ();
		}
		//PLAYERS
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		foreach (var p in players) {

			Player player = p.GetComponent<Player> ();

			bool found = false;
			for (int i = 0; i < worldData.playersName.Length; i++) {
				if (player.playerName == worldData.playersName [i]) {
					found = true;
					player.transform.position = worldData.playerPositions [i];

					worldData.playerInventories [i].DeserializePlayerInventory(player.GetComponent<PlayerInventory> ());

				}
			}
			/*
			if (!found) {
				worldData.playersName.Add (player.playerName);
				worldData.playerPositions.Add(player.transform.position);
				worldData.playerInventories.Add(new SerializedPlayerInventory (player.GetComponent<PlayerInventory> ()));
			}
			*/
		}

		//SEEDS
		Debug.Log("nb of seeds : " + worldData.seeds.Length);

		for (int i = 0; i < worldData.seeds.Length; i++) {

			//PlantSeed s = Instantiate (GameManager.gm.pm.seedsPrefab, worldData.seeds [i].position, Quaternion.identity).GetComponent<PlantSeed> ();

			GameManager.gm.pm.plantSeeds[i].indexInPlantManager = worldData.seeds [i].plantTypeIndex;
			GameManager.gm.pm.plantSeeds[i].gameObject.SetActive (worldData.seeds [i].active);
			print(worldData.seeds [i].active);
		}






		CanvasManager.cm.multiplayerMenu.multiplayerClientLoadingPlants.SetActive (false);
	}

}


[System.Serializable]
public class WorldData
{
	public SerializedPlant[] plants;


	public SerializedPlantSeed[] seeds;

	public string[] playersName;//OK
	public Vector3[] playerPositions;//OK
	public SerializedPlayerInventory[] playerInventories;//OK

	public WorldData()
	{
		//plants = new List<SerializedPlant> ();
	}

}

[System.Serializable]
public struct SerializedPlayerInventory
{
	public int[] indexesInPlantManager;
	public int[] numberOfSeeds;

	public SerializedPlayerInventory (PlayerInventory playerInventory)
	{
		indexesInPlantManager = new int[playerInventory.NbOfSeeds];
		numberOfSeeds = new int[playerInventory.NbOfSeeds];
		
		for (int i = 0; i < indexesInPlantManager.Length; i++) {
			
			indexesInPlantManager [i] = playerInventory.inventory [i].plantSeed.indexInPlantManager;
			numberOfSeeds [i] = playerInventory.inventory [i].number;
		}
	}


	/*
	public void AddSeed(PlantSeed seed)
	{
		for (int i = 0; i < indexesInPlantManager.Count; i++) {
			if (indexesInPlantManager [i] == seed.indexInPlantManager) {
				numberOfSeeds++;
				return;
			}
		}

		indexesInPlantManager.Add (seed.indexInPlantManager);
		numberOfSeeds.Add (1);
	}
	*/

	public PlantSeed DeserializeSeedForInventory(int index)
	{
		GameObject seed = new GameObject ();
		PlantSeed ps = seed.AddComponent<PlantSeed> ();
		ps.indexInPlantManager = indexesInPlantManager [index];
		return ps;
	}

	public void DeserializePlayerInventory(PlayerInventory playerInventory)
	{
		playerInventory.inventory = new List<PlantSeedInventory> (indexesInPlantManager.Length);

		for (int i = 0; i < indexesInPlantManager.Length; i++) {

			PlantSeed ps = DeserializeSeedForInventory (i);
			ps.gameObject.SetActive (false);

			playerInventory.inventory [i] = new PlantSeedInventory (ps, numberOfSeeds [i]);
		}
	}

}

[System.Serializable]
public struct SerializedPlant
{
	public int plantTypeIndex;
	public float plantTime;
	public Vector3 initialPosition;
	public Vector3 initialDirection;

	public int seed;

	public int indexInGameData;

	public SerializedPlant(int plantTypeIndex, float plantTime, Vector3 initialPosition, Vector3 initialDirection, int seed, int indexInGameData)
	{
		this.plantTypeIndex = plantTypeIndex;
		this.plantTime = plantTime;
		this.initialPosition = initialPosition;
		this.initialDirection = initialDirection;
		this.seed = seed;
		this.indexInGameData = indexInGameData;
	}
}

[System.Serializable]
public struct SerializedPlantSeed
{
	public int plantTypeIndex;
	public Vector3 position;
	public bool active;

	public SerializedPlantSeed(int plantTypeIndex, Vector3 position, bool active)
	{
		this.plantTypeIndex = plantTypeIndex;
		this.position = position;
		this.active = active;
	}
}