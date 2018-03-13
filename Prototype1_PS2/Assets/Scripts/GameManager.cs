using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gm;

	public CanvasManager cManager;

	public bool isGamePaused()
	{
		return cManager.toggled;
	}


	void Awake() {

		if (gm == null)
			gm = this;
		else if (gm != this)
			Destroy (gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
