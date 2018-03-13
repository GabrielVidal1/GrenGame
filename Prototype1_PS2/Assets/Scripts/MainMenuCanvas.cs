using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuCanvas : MonoBehaviour {

	public GameObject mainMenuPanel;

	public GameObject optionPanel;
	public Slider volumeSlider;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void GoToMainMenu()
	{
		mainMenuPanel.SetActive (true);
		optionPanel.SetActive (false);
	}


	public void Play()
	{
		SceneManager.LoadScene ("Main");

	}

	public void GoToOptionPanel()
	{
		mainMenuPanel.SetActive (false);
		optionPanel.SetActive (true);
	}


	public void QuitGame()
	{
		Application.Quit ();

	}

	public void UpdateVolume()
	{
		AudioListener.volume = volumeSlider.value;
	}

}
