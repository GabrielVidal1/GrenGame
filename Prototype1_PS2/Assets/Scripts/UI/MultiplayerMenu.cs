using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MultiplayerMenu : MonoBehaviour {

	[SerializeField] private GameObject multiplayerClientWaitingScreen;
	[SerializeField] private GameObject multiplayerMenuPanel;

	[SerializeField] private GameObject mainMenu;
	[SerializeField] private WorldSelectionPanel worldSelectionPanel;

	public TMP_InputField ipAddressInputField;

	[SerializeField] private GameObject loadingScreen;
	[SerializeField] private Slider loadingScreenLoadingBar;

	[SerializeField] private MainMainMenu mainMainMenu;

	private bool tryToConnectToServer = false;
	private bool hasLaunched = false;



	void Update()
	{
		if (!hasLaunched) {
			if (tryToConnectToServer) {
				if (!GameManager.gm.isHost) {

					//Debug.Log ("j'essaie de me connecter  : " + GameManager.gm.IsClientConnected ());

					if (GameManager.gm.IsClientConnected ()) {

						//Debug.Log ("client connection adress : " + GameManager.gm.clientConnection.address);
						hasLaunched = true;

						multiplayerClientWaitingScreen.SetActive (false);
						//CanvasManager.cm.inGameMenu.gameObject.SetActive (true);
						Cursor.lockState = CursorLockMode.Locked;

						loadingScreen.SetActive (true);

						GameManager.gm.LoadScene (loadingScreenLoadingBar, GameManager.gm.mainSceneName);
					}
				}
			}
		}
	}



	public void BackToMainMenu()
	{
		mainMainMenu.transition = VoidBackToMainMenu;
		mainMainMenu.Transit ();
	}

	void VoidBackToMainMenu()
	{
		mainMenu.SetActive (true);
		gameObject.SetActive (false);
	}

	public void LaunchHost()
	{
		mainMainMenu.transition = VoidLaunchHost;
		mainMainMenu.Transit ();
	}

	public void VoidLaunchHost()
	{
		GameManager.gm.PrepareLaunching (true);
		worldSelectionPanel.gameObject.SetActive (true);
		//worldSelectionPanel.ResetVerticalScrollBar ();
		gameObject.SetActive (false);
	}



	public void LaunchClient()
	{
		mainMainMenu.transition = VoidLaunchClient;
		mainMainMenu.Transit ();
	}

	void VoidLaunchClient()
	{
		tryToConnectToServer = true;
		multiplayerMenuPanel.SetActive (false);
		multiplayerClientWaitingScreen.SetActive (true);

		GameManager.gm.ipAddressToJoin = ipAddressInputField.text;
		GameManager.gm.PrepareLaunching (false);
		GameManager.gm.Launch ();
	}


	public void CancelLauchClient()
	{
		tryToConnectToServer = false;
		GameManager.gm.StopClient ();
		multiplayerClientWaitingScreen.SetActive (false);
		multiplayerMenuPanel.SetActive (true);

	}
}
