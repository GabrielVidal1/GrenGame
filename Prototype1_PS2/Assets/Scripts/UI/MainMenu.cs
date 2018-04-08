using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public string mainSceneName;

	[SerializeField] private GameObject worldSelectionPanel;
	[SerializeField] private GameObject multiplayerPanel;

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
		multiplayerPanel.SetActive (true);
		gameObject.SetActive (false);
	}
}
