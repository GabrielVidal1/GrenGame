using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

	public static GameManager gm;
	static GameData gameData;


	public Player localPlayer;

	public NetworkManager nm;
	public PlantManager pm;

	public bool isHost;

	public string ipAddressToJoin;

	void Awake() {

		if (gm == null)
			gm = this;
		else if (gm != this)
			Destroy (gameObject);

		nm = GetComponent<NetworkManager> ();
		pm = GetComponent<PlantManager> ();
	}


	public void Launch(bool isHost)
	{
		this.isHost = isHost;
		gameData = new GameData ();


		if (isHost) {
			CanvasManager.cm.genericCamera.SetActive (false);
			Debug.Log ("Start Host");

			nm.StartHost ();
		} else {
			Debug.Log ("Start Client");

			nm.networkAddress = ipAddressToJoin;
			Debug.Log (ipAddressToJoin);
			nm.networkPort = 7777;
			nm.StartClient ();
		}
	}

	public void StopGame()
	{
		CanvasManager.cm.genericCamera.SetActive (true);

		localPlayer.DisablePlayer ();

		if (isHost) {
			Debug.Log ("Stop Host");
			nm.StopHost ();
		} else {
			Debug.Log ("Stop Client");
			StopClient ();
		}
	}

	public void StopClient()
	{
		nm.StopClient ();
	}

	public bool IsClientConnected()
	{
		return nm.IsClientConnected ();
	}

	public static void SavePlantTime(int plantIndex, float time)
	{
		PlantSave ps = gameData.plants [plantIndex];
		ps.plantTime = time;
		gameData.plants [plantIndex] = ps;
		//Debug.Log ("Updated Plant " + plantIndex.ToString ());
	}

	public static void Save()
	{

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create( Application.persistentDataPath + "/GameData.dat");

		bf.Serialize( file, gameData );
		file.Close();

		Debug.Log ("Game Succesfuly saved!");
	}

	public void LoadPlants(PlantSave[] plantsSave)
	{
		CanvasManager.cm.multiplayerMenu.multiplayerClientLoadingPlants.SetActive (true);

		foreach (PlantSave plantSave in plantsSave) {
			LoadPlant(plantSave);
		}

		CanvasManager.cm.multiplayerMenu.multiplayerClientLoadingPlants.SetActive (false);
	}


	private void LoadPlant(PlantSave plantSave)
	{
		Plant plant = Instantiate (pm.plantsPrefabs [plantSave.plantTypeIndex], plantSave.initialPosition, Quaternion.identity).GetComponent<Plant> ();
		plant.initialDirection = plantSave.initialDirection;
		plant.SetSeed (plantSave.seed);
		plant.time = plantSave.plantTime;
		plant.indexInGameData = plantSave.indexInGameData;

		pm.plants.Add (plant);


		plant.InitializePlant ();



		gameData.plants.Add (plantSave);


	}

	public void CreateNewPlant(int plantTypeIndex, float plantTime, Vector3 initialPosition, Vector3 initialDirection, int seed)
	{
		PlantSave ps = new PlantSave (plantTypeIndex, 0f, initialPosition, initialDirection, seed, gameData.plants.Count);
		LoadPlant (ps);
	}

	public PlantSave[] GetPlantArrayToTransmit()
	{
		return gameData.plants.ToArray ();
	}
}

public struct PlantSave
{
	public int plantTypeIndex;
	public float plantTime;
	public Vector3 initialPosition;
	public Vector3 initialDirection;

	public int seed;

	public int indexInGameData;

	public PlantSave(int plantTypeIndex, float plantTime, Vector3 initialPosition, Vector3 initialDirection, int seed, int indexInGameData)
	{
		this.plantTypeIndex = plantTypeIndex;
		this.plantTime = plantTime;
		this.initialPosition = initialPosition;
		this.initialDirection = initialDirection;
		this.seed = seed;
		this.indexInGameData = indexInGameData;
	}
}

[Serializable]
class GameData
{
	public List<PlantSave> plants;

	public GameData()
	{
		plants = new List<PlantSave> ();
	}

}