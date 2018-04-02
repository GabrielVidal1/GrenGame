using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

	public string mainSceneName;

	[SerializeField] private GameObject worldSelectionPanel;

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
		worldSelectionPanel.SetActive (true);
		gameObject.SetActive (false);
    }
}
