using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

	public string mainSceneName;


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
		SceneManager.LoadScene(mainSceneName);
    }
}
