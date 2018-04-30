using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InGameMenu : MonoBehaviour {

	public GameObject pauseMenuUI;

	public GameObject inGameOverlay;

	public bool isPaused = false;


	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TogglePause (!isPaused);
        }
	}

	public void TogglePause(bool toggle)
	{
		isPaused = toggle;
		pauseMenuUI.SetActive (isPaused);
		inGameOverlay.SetActive (!isPaused);

		if (isPaused) {

			CanvasManager.cm.playerInventoryGrid.CloseInventory ();

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
			//Debug.Log ("je quitte le menu pause");
		}
	}


	public void LoadMultiplayerMenu()
	{
		GameManager.gm.StopGame ();
	}

}
