using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour {

	public static CanvasManager cm;
	//public GameObject genericCamera;
	public string mainMenuSceneName;
	
	
	public InGameMenu inGameMenu;

	public GameObject loadingScreen;
	public Slider loadingScreenLoadingBar;


	public GameObject disconnectionScreen;
	//public MultiplayerMenu multiplayerMenu;

	public SeedSelectionWheel seedSelectionWheel;

	public PlayerInventoryGrid playerInventoryGrid;


	void Awake() {

		if (cm == null)
			cm = this;
		else if (cm != this)
			Destroy (gameObject);

	}

	public void ExitAndSaveGame()
	{
		GameManager.gm.SaveAndExit ();
	}

	public void GoToMainMenu()
	{
		GameManager.gm.GoToMainMenu ();
	}

}
