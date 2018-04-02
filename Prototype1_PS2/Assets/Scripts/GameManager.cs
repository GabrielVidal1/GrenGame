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
	public WorldSerialization wd;

	public bool isHost;

	public string ipAddressToJoin;

	void Awake() {

		if (gm == null)
			gm = this;
		else if (gm != this)
			Destroy (gameObject);

		nm = GetComponent<NetworkManager> ();
		pm = GetComponent<PlantManager> ();
		wd = GetComponent<WorldSerialization> ();

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
	/*
	public void LoadPlants(SerializedPlant[] serializedPlants)
	{
		CanvasManager.cm.multiplayerMenu.multiplayerClientLoadingPlants.SetActive (true);

		foreach (SerializedPlant serializedPlant in serializedPlants) {
			LoadPlant(serializedPlant);
		}

		CanvasManager.cm.multiplayerMenu.multiplayerClientLoadingPlants.SetActive (false);
	}


	void LoadPlant(SerializedPlant serializedPlant)
	{
		Plant plant = Instantiate (pm.plantsPrefabs [serializedPlant.plantTypeIndex], serializedPlant.initialPosition, Quaternion.identity).GetComponent<Plant> ();
		plant.initialDirection = serializedPlant.initialDirection;
		plant.SetSeed (serializedPlant.seed);
		plant.time = serializedPlant.plantTime;
		plant.indexInGameData = serializedPlant.indexInGameData;

		pm.plants.Add (plant);


		plant.InitializePlant ();



		WorldSerialization.worldData.plants.Add (serializedPlant);


	}

	public void CreateNewPlant(int plantTypeIndex, float plantTime, Vector3 initialPosition, Vector3 initialDirection, int seed)
	{
		LoadPlant (ps);
	}
	*/
}



[Serializable]
class GameData
{
	public List<SerializedPlant> plants;

	public GameData()
	{
		plants = new List<SerializedPlant> ();
	}

}