using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InGameMenu : MonoBehaviour {

	public GameObject pauseMenuUI;

	public GameObject inGameOverlay;

	public TextMeshProUGUI selectedPlant;

	public bool isPaused = false;


	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TogglePause (!isPaused);
		}

		if (!isPaused)
			Cursor.lockState = CursorLockMode.Locked;

	}

	public void TogglePause(bool toggle)
	{
		isPaused = toggle;
		pauseMenuUI.SetActive (isPaused);
		inGameOverlay.SetActive (!isPaused);

		if (isPaused)
			Cursor.lockState = CursorLockMode.None;
		else {
			Cursor.lockState = CursorLockMode.Locked;
			//CanvasManager.cm.seedSelectionWheel.ReactivateFromInactiveState ();
		}
	}


	public void LoadMultiplayerMenu()
	{
		GameManager.gm.StopGame ();
	}

}
