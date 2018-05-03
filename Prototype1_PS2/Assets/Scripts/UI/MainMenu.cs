using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public string mainSceneName;

	[SerializeField] private GameObject worldSelectionPanel;
	[SerializeField] private GameObject multiplayerPanel;
	[SerializeField] private GameObject optionPanel;

	[SerializeField] private MainMainMenu mainMainMenu;

	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Quit()
	{
		Debug.Log("Quit");
		Application.Quit();
	}

	public void PlayGame()
	{
		mainMainMenu.transition = VoidPlayGame;
		mainMainMenu.Transit ();
	}

	void VoidPlayGame()
	{
		multiplayerPanel.SetActive (true);
		gameObject.SetActive (false);
	}


	public void Option()
	{
		mainMainMenu.transition = VoidOption;
		mainMainMenu.Transit ();
	}

	void VoidOption()
	{
		optionPanel.SetActive (true);
		gameObject.SetActive (false);
	}
}
