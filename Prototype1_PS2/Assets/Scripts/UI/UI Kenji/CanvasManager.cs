using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour {

	#region SINGLETON
	public static CanvasManager cm;

	void Awake() {

		if (cm == null)
			cm = this;
		else if (cm != this)
			Destroy (gameObject);

	}

	#endregion

	public GameObject genericCamera;
	public string mainMenuSceneName;
	//public bool isPaused = false;




	public InGameMenu inGameMenu;
	public MultiplayerMenu multiplayerMenu;


	/*
	public GameObject multiplayerClientWaitingScreen;

	public TMP_InputField ipAddressInputField;


	public bool inGame;

	void Update() {
		if (inGame) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				TogglePause (!isPaused);
			}
		} else {
			if (!GameManager.gm.isHost) {
				if (GameManager.gm.IsClientConnected ()) {
					inGame = true;
					multiplayerClientWaitingScreen.SetActive (false);

					Debug.Log ("CONNECTED");
				}
			}
		}
	}

	public void TogglePause(bool toggle)
	{
		isPaused = toggle;
		inGameTab.SetActive (isPaused);

		if (isPaused)
			Cursor.lockState = CursorLockMode.None;
		else
			Cursor.lockState = CursorLockMode.Locked;
	}




	public void LoadMainMenu()
	{
		SceneManager.LoadScene(mainMenuSceneName);
		inGame = false;
	}

	public void LoadMultiplayerMenu()
	{
		multiplayerTab.SetActive (true);
		inGameTab.SetActive (false);

		GameManager.gm.StopGame ();
		inGame = false;

	}


	public void Quit()
	{
		Debug.Log("Quit");
		Application.Quit();
	}

	//MULTIPLAYER

	public void LaunchHost()
	{
		GameManager.gm.Launch (true);
		multiplayerTab.SetActive (false);
		inGame = true;
	}

	public void LaunchClient()
	{
		multiplayerTab.SetActive (false);
		multiplayerClientWaitingScreen.SetActive (true);

		Debug.Log ("Before the connection   " + ClientScene.ready);

		GameManager.gm.ipAddressToJoin = ipAddressInputField.text;
		GameManager.gm.Launch (false);
	}

	public void CancelLauchClient()
	{
		GameManager.gm.StopClient ();
		multiplayerClientWaitingScreen.SetActive (false);
		multiplayerTab.SetActive (true);

	}
*/
}
