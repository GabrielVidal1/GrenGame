using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MultiplayerMenu : MonoBehaviour {

	public GameObject multiplayerClientWaitingScreen;

	public GameObject multiplayerTab;

	public TMP_InputField ipAddressInputField;


	void Update()
	{
		if (!GameManager.gm.isHost) {
			
			if (GameManager.gm.IsClientConnected ()) {
				
				multiplayerClientWaitingScreen.SetActive (false);
				CanvasManager.cm.inGameMenu.gameObject.SetActive (true);

				Cursor.lockState = CursorLockMode.Locked;

				gameObject.SetActive (false);
			}
		}


	}




	public void LoadMainMenu()
	{
		SceneManager.LoadScene(CanvasManager.cm.mainMenuSceneName);
	}

	public void LaunchHost()
	{
		GameManager.gm.Launch (true);
		CanvasManager.cm.inGameMenu.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	public void LaunchClient()
	{
		multiplayerTab.SetActive (false);
		multiplayerClientWaitingScreen.SetActive (true);

		GameManager.gm.ipAddressToJoin = ipAddressInputField.text;
		GameManager.gm.Launch (false);
	}

	public void CancelLauchClient()
	{
		GameManager.gm.StopClient ();
		multiplayerClientWaitingScreen.SetActive (false);
		multiplayerTab.SetActive (true);

	}
}
