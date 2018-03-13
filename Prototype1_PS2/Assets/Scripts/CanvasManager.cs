using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour {

	public GameObject genericCamera;

	public GameObject pausePanel;


	public bool toggled;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		//if (!isLocalPlayer)
		//	return;

		if (Input.GetKeyDown (KeyCode.P)) {
			toggled = !toggled;

			pausePanel.SetActive (toggled);

			if (toggled)
				Cursor.lockState = CursorLockMode.None;
			else
				Cursor.lockState = CursorLockMode.Locked;

		}




		
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene ("MainMenu");

	}

}
