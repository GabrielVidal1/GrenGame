using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterPanelsManager : MonoBehaviour {

	[SerializeField] private GameObject materialWindow;
	[SerializeField] private GameObject parameterWindow;

	[SerializeField] private ImageLoader imageLoader;




	public void Initialize()
	{
		parameterWindow.SetActive (true);
		materialWindow.SetActive (false);

		imageLoader.LoadTextures ();
	}

	public void ToggleMaterialWindow()
	{
		parameterWindow.SetActive (false);
		materialWindow.SetActive (true);
	}

	public void ToggleParameterWindow()
	{
		parameterWindow.SetActive (true);
		materialWindow.SetActive (false);
	}

	void Start () 
	{
		Initialize ();
	}
}
