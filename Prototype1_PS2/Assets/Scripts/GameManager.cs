using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : NetworkBehaviour {

	public static GameManager gm;

	public string mainSceneName;
	public string mainMenuSceneName;
	public string plantEditorSceneName;

	public Player localPlayer;
	public string localPlayerName;


	public NetworkManager nm;
	public PlantManager pm;
	public WorldSerialization wd;
	public ZoneAndDoorManager zd;
	public GeneticCrossing gc;

	public bool isHost;

	public string ipAddressToJoin;

	public string worldName;


	public NetworkConnection clientConnection;

	private bool isMain;
	void Awake() {

		if (gm == null)
			gm = this;
		else if (gm != this) {
			if (isMain) {
				Destroy (gm);
				gm = this;
			} else {
				Destroy (gameObject);
			}
		}

		isMain = true;

		nm = GetComponent<NetworkManager> ();
		pm = GetComponent<PlantManager> ();
		wd = GetComponent<WorldSerialization> ();
		zd = GetComponent<ZoneAndDoorManager> ();
		gc = GetComponent<GeneticCrossing> ();

	}

	public string GetPlayerName()
	{
		localPlayerName = clientConnection.address;
		return localPlayerName;

	}

	public void LaunchNewWorld(string worldName, Slider loadingScreenLoadingBar)
	{
		SetWorld (worldName);
		LoadScene (loadingScreenLoadingBar, mainSceneName);
	}


	public void SetWorld(string worldName)
	{
		wd.LoadWorldFile (worldName);
		this.worldName = worldName;
	}

	public void LoadScene(Slider loadingScreenLoadingBar, string sceneName)
	{
		StartCoroutine(_LoadMainScene(loadingScreenLoadingBar, sceneName));
	}

	IEnumerator _LoadMainScene(Slider loadingScreenLoadingBar, string sceneName)
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);

		Debug.Log ("loading started !");

		while (!loading.isDone) {
			yield return null;
			loadingScreenLoadingBar.value = Mathf.Clamp01( loading.progress / 0.9f);
		}
		Debug.Log ("loading END !");

		if (sceneName == mainSceneName) {
			if (isHost) {
				Launch ();


			} else {


				ClientScene.Ready (clientConnection);
				ClientScene.AddPlayer (0);


			}
		}
		//else if (sceneName == mainMenuSceneName)
			//DO THING

	}

	public void PrepareLaunching(bool isHost)
	{
		this.isHost = isHost;
	}




	public void Launch()
	{

		if (isHost) {
			//CanvasManager.cm.genericCamera.SetActive (false);
			Debug.Log ("Start Host");
			wd.DeserializeWorld (wd.worldData);

			nm.StartHost ();

		} else {
			Debug.Log ("Start Client");

			nm.networkAddress = ipAddressToJoin;
			Debug.Log (ipAddressToJoin);
			nm.networkPort = 7777;

			nm.StartClient ();

		}



		//Debug.Log ("I launched from :" + isHost);
	}

	public void OnServerStop()
	{
		if (isHost) {

		} else {
			CanvasManager.cm.disconnectionScreen.SetActive (true);
		}
	}

	public void CmdClientDisconnectionServerSave ()
	{
		//Debug.Log ("je sauvegarde la partie parce qu'un joueur est parti");

		wd.SerializeWorld ();
		wd.SaveWorldFile ();
	}



	public void GoToMainMenu()
	{
		CanvasManager.cm.loadingScreen.SetActive (true);

		LoadScene (CanvasManager.cm.loadingScreenLoadingBar, mainMenuSceneName);
	}

	public void Save()
	{
		wd.SerializeWorld ();
		wd.SaveWorldFile ();
		Debug.Log ("world saved !");
	}

	public void SaveAndExit()
	{
		if (isHost) {
			Save ();
		} else {
			//Debug.Log ("finished saving on server");
			//Debug.LogError ("je sauve la partie depuis le joueur :)");

		}

		StopGame ();

		GoToMainMenu ();
	}

	public void StopGame()
	{
		//localPlayer.DisablePlayer ();
		pm.ResetLists();
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


