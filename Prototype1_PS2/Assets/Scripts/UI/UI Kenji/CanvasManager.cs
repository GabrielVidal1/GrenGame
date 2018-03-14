using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour {

	public static CanvasManager cm;
	public GameObject genericCamera;
	public string mainMenuSceneName;
	
	
	public InGameMenu inGameMenu;
	public MultiplayerMenu multiplayerMenu;

	void Awake() {

		if (cm == null)
			cm = this;
		else if (cm != this)
			Destroy (gameObject);

	}



}
