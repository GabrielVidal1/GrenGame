using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InGameMenu : MonoBehaviour {


	public GameObject inGameOverlay;


	[SerializeField] private GameObject optionMenu;

	[SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject controlsMenu;


    [SerializeField] private MainMainMenu mainMainMenu;


	[SerializeField] private RawImage darkBack;

	public bool isPaused = false;

	bool inOptionMenu;

	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !inOptionMenu) {
			TogglePause (!isPaused);
        }
	}

	public void GoToOptions()
	{
		mainMainMenu.transition = VoidGoToOptions;
		mainMainMenu.Transit ();
	}

	void VoidGoToOptions()
	{
		optionMenu.SetActive (true);
		pauseMenu.SetActive (false);
		inOptionMenu = true;
	}

    public void GoToControls()
    {
        mainMainMenu.transition = VoidGoToControls;
        mainMainMenu.Transit();
    }

    void VoidGoToControls()
    {
        controlsMenu.SetActive(true);
        optionMenu.SetActive(false);
        inOptionMenu = true;
    }

    public void FromOptionMenuToPauseMenu()
	{
		inOptionMenu = false;
	}

	public void TogglePause(bool toggle)
	{
		isPaused = toggle;
		pauseMenu.SetActive (isPaused);
		inGameOverlay.SetActive (!isPaused);
		darkBack.gameObject.SetActive (toggle);
		if (isPaused) {

			CanvasManager.cm.playerInventoryGrid.CloseInventory ();

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Debug.Log ("je quitte le menu pause");
        }
    }


	public void LoadMultiplayerMenu()
	{
		GameManager.gm.StopGame ();
	}

}
