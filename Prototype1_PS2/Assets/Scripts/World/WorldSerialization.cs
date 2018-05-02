using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class WorldSerialization : MonoBehaviour{

	public WorldData worldData;

	public void FirstStart()
	{
		
		worldData = new WorldData ();

	}


	public void SaveWorldFile()
	{
		if (worldData == null)
			FirstStart ();

		if (GameManager.gm.worldName != "") {

			//SAVING THE FILE
			string path = Application.persistentDataPath + "/Worlds/" + GameManager.gm.worldName + ".grenworld";

			//Debug.Log (path);
			BinaryFormatter bf = new BinaryFormatter ();


			FileStream file = File.Create (path);

			bf.Serialize (file, worldData);
			file.Close ();
		}
	}

	public void LoadWorldFile(string worldName)
	{
		if (Directory.Exists (Application.persistentDataPath + "/Worlds")) {
			if (File.Exists (Application.persistentDataPath + "/Worlds/" + worldName + ".grenworld")) {

				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/Worlds/" + worldName + ".grenworld", FileMode.Open);


				//TRY CATCH

				worldData = (WorldData)bf.Deserialize (file);
				file.Close ();

			} else {
				worldData = new WorldData ();
			}
		} else {
			throw new UnityException ("directory world cannot be found");
		}

	}

	public void SerializeWorld()
	{
		(new GameObject ("Before serilisation")).AddComponent<Test> ().wd = new WorldData(worldData);


		//Debug.Log ("je serialise le monde");

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		List<string> playersName = new List<string>(worldData.playersName);

		//Debug.Log (worldData.playersName.Length + " ----------  " + playersName.Count);


		List<SerializedVector3> playerPositions = new List<SerializedVector3> (worldData.playerPositions);
		List<SerializedPlayerInventory> playerInventories = new List<SerializedPlayerInventory> (worldData.playerInventories);




		foreach (var p in players) {

			Player player = p.GetComponent<Player> ();

			if (player.synched) {
				int index = -1;
				for (int i = 0; i < playersName.Count; i++) {
					if (player.playerName == playersName [i]) {
						index = i;
						playerPositions [i] = new SerializedVector3 (player.transform.position);

						playerInventories [i] = new SerializedPlayerInventory (player.GetComponent<PlayerInventory> ());
					}
				}
				if (index == -1) {
					playersName.Add (player.playerName);
					playerPositions.Add (new SerializedVector3 (player.transform.position));
					playerInventories.Add (new SerializedPlayerInventory (player.GetComponent<PlayerInventory> ()));
				}
			}
		}

		worldData.playersName = playersName.ToArray ();
		worldData.playerPositions = playerPositions.ToArray ();
		worldData.playerInventories = playerInventories.ToArray ();


		GameManager.gm.pm.SerializePlants (worldData);
		//Debug.Log(worldData.plants.Length);


		GameManager.gm.pm.SerializeSeeds (worldData);
		//Debug.Log(worldData.seeds.Length);

		GameManager.gm.zd.Init ();
		GameManager.gm.zd.SerializeDoors(worldData);
		GameManager.gm.zd.SerializeZones(worldData);

		(new GameObject ("After serilisation")).AddComponent<Test> ().wd = new WorldData(worldData);

		//(new GameObject ()).AddComponent<Test> ().wd = worldData;

	}


	public void LoadPlayerInformation(Player player)
	{
		//Debug.LogError ("je sync l'inventaire de "+ player.playerName);

		for (int i = 0; i < worldData.playersName.Length; i++) {
			if (player.playerName == worldData.playersName [i]) {

				//Debug.Log ("je remplis l'inventaire de " + player.playerName);

				player.transform.position = worldData.playerPositions [i].Deserialize();

				PlayerInventory playerInventory = player.GetComponent<PlayerInventory> ();
				SerializedPlayerInventory serializedPlayerInventory = worldData.playerInventories [i];

				playerInventory.inventory.Clear ();
				playerInventory.inventory = new List<PlantSeedInventory> (serializedPlayerInventory.indexesInPlantManager.Length);

				for (int j = 0; j < serializedPlayerInventory.indexesInPlantManager.Length; j++) {
					playerInventory.inventory.Add(new PlantSeedInventory (serializedPlayerInventory.indexesInPlantManager [j], serializedPlayerInventory.numberOfSeeds [j]));
				}

				if (serializedPlayerInventory.indexesInPlantManager.Length > 0) {
					CanvasManager.cm.seedSelectionWheel.SetPlayer (player.GetComponent<PlayerInventory>());
				}
			}
		}

		player.synched = true;


	}




	public void DeserializeWorld(WorldData worldData)
	{

		//Debug.Log ("I'm deserializing the world save");


		//CanvasManager.cm.multiplayerMenu.multiplayerClientLoadingPlants.SetActive (true);

		//PLANTS

		GameManager.gm.pm.plants.Clear ();
		GameManager.gm.pm.plants = new List<Plant> ();

		foreach (SerializedPlant serializedPlant in worldData.plants) {
			
			Plant plant = Instantiate (GameManager.gm.pm.plantsPrefabs [serializedPlant.plantTypeIndex], 
				serializedPlant.initialPosition.Deserialize(), 
				Quaternion.identity).GetComponent<Plant> ();
			
			plant.initialDirection = serializedPlant.initialDirection.Deserialize();

			plant.SetSeed (serializedPlant.seed);
			plant.time = serializedPlant.plantTime;
			//plant.indexInGameData = serializedPlant.indexInGameData;
			plant.InitializePlant ();


			GameManager.gm.pm.plants.Add (plant);
		}



		//SEEDS
		GameManager.gm.pm.plantSeeds.Clear ();
		GameManager.gm.pm.plantSeeds = new List<PlantSeed> ();

		for (int i = 0; i < worldData.seeds.Length; i++) {

			PlantSeed s = Instantiate (GameManager.gm.pm.seedsPrefab[worldData.seeds [i].plantTypeIndex], worldData.seeds [i].position.Deserialize(), Quaternion.identity).GetComponent<PlantSeed> ();
			s.indexInPlantManager = worldData.seeds [i].plantTypeIndex;
			s.name = "RELOADED_SEED_" + i.ToString();			
			GameManager.gm.pm.plantSeeds.Add (s);
		}


		GameManager.gm.zd.Init ();
		GameManager.gm.zd.DeserializeZones (worldData);
		GameManager.gm.zd.DeserializeDoors (worldData);




		this.worldData = worldData;

		if (!GameManager.gm.isHost)
			LoadPlayerInformation (GameManager.gm.localPlayer);

	}

}


[System.Serializable]
public class WorldData
{
	//public bool firstSave;


	public SerializedPlant[] plants;//OK


	public SerializedPlantSeed[] seeds;//OK

	public string[] playersName;//OK
	public SerializedVector3[] playerPositions;//OK
	public SerializedPlayerInventory[] playerInventories;//OK

	public SerializedZone[] zones;
	public SerializedDoor[] doors;


	public WorldData()
	{
		plants = new SerializedPlant[0];
		seeds = new SerializedPlantSeed[0];;

		playersName = new string[0];
		playerPositions = new SerializedVector3[0];
		playerInventories = new SerializedPlayerInventory[0];
		zones = new SerializedZone[0];
		doors = new SerializedDoor[0];
	}

	public WorldData(WorldData clone)
	{
		plants = (SerializedPlant[])clone.plants.Clone ();
		seeds = (SerializedPlantSeed[])clone.seeds.Clone ();

		playersName = (string[])clone.playersName.Clone ();
		playerPositions = (SerializedVector3[])clone.playerPositions.Clone ();
		playerInventories = (SerializedPlayerInventory[])clone.playerInventories.Clone ();
		zones = (SerializedZone[])clone.zones.Clone ();
		doors = (SerializedDoor[])clone.doors.Clone ();

	}

}

[System.Serializable]
public struct SerializedDoor
{
	public bool open;

	public SerializedDoor (Door door)
	{
		open = door.IsOpen;
	}
}

[System.Serializable]
public struct SerializedZone
{
	public int nbOfPoints;

	public SerializedZone (Zone zone)
	{
		nbOfPoints = zone.TotalPoints();
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
			
			indexesInPlantManager [i] = playerInventory.inventory [i].plantSeedIndexInPlantManager;
			numberOfSeeds [i] = playerInventory.inventory [i].number;
		}
	}
}

[System.Serializable]
public struct SerializedPlant
{
	public int plantTypeIndex;
	public float plantTime;
	public SerializedVector3 initialPosition;
	public SerializedVector3 initialDirection;

	public int seed;

	public int indexInGameData;

	public SerializedPlant(int plantTypeIndex, float plantTime, Vector3 initialPosition, Vector3 initialDirection, int seed, int indexInGameData)
	{
		this.plantTypeIndex = plantTypeIndex;
		this.plantTime = plantTime;
		this.initialPosition = new SerializedVector3(initialPosition);
		this.initialDirection = new SerializedVector3 (initialDirection);
		this.seed = seed;
		this.indexInGameData = indexInGameData;
	}
}

[System.Serializable]
public struct SerializedPlantSeed
{
	public int plantTypeIndex;
	public SerializedVector3 position;

	public bool active;

	public SerializedPlantSeed(int plantTypeIndex, Vector3 position, bool active)
	{
		this.plantTypeIndex = plantTypeIndex;
		this.position = new SerializedVector3(position);
		this.active = active;
	}
}

[System.Serializable]
public struct SerializedKeyFrame
{
	public float inTangent;
	public float outTangent;
	public float time;
	public float value;

	public int tangentMode;


	public SerializedKeyFrame(Keyframe kf)
	{
		inTangent = kf.inTangent;
		outTangent = kf.outTangent;
		time = kf.time;
		value = kf.value;

		tangentMode = (int)kf.tangentMode;

	}

	public Keyframe Deserialize()
	{
		return new Keyframe (time, value, inTangent, outTangent);
	}

}

[System.Serializable]
public struct SerializedAnimationCurve
{
	public SerializedKeyFrame[] keys;

	public SerializedAnimationCurve(AnimationCurve c)
	{
		keys = new SerializedKeyFrame[c.keys.Length];
		for (int i = 0; i < c.keys.Length; i++) {
			keys [i] = new SerializedKeyFrame (c.keys [i]);
		}
	}

	public AnimationCurve Deserialize()
	{
		Keyframe[] k = new Keyframe[keys.Length];
		for (int i = 0; i < k.Length; i++) {
			k [i] = keys [i].Deserialize ();
		}
		return new AnimationCurve (k);
	}
}

[System.Serializable]
public struct SerializedVector3
{
	public float x;
	public float y;
	public float z;

	public SerializedVector3(Vector3 vector3)
	{
		x = vector3.x;
		y = vector3.y;
		z = vector3.z;
	}

	public Vector3 Deserialize()
	{
		return new Vector3 (x, y, z);
	}
}

