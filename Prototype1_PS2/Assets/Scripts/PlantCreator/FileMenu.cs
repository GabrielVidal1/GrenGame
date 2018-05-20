using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FileMenu : MonoBehaviour {

	[SerializeField] private Button FileButton; 

	[SerializeField] private GameObject filePanel;

	[SerializeField] private PlantPartPanelManager plantPartPanelManager;

	[SerializeField] private OpenPanel openPanel;

	[SerializeField] private PlantSerializer plantSerializer;

	public bool unsavedChanges;

	bool opened;

	
	public void Initialize()
	{
		filePanel.SetActive (false);

	}


	bool IsValidPlant(string path)
	{
		//string extension = path.Substring (path.LastIndexOf ("."), path.Length - ".grenplant".Length);
		//Debug.Log (extension);

		return true;
	}



	void Start()
	{
		Initialize ();
	}


	void Update()
	{
		if (Input.GetKey (KeyCode.LeftControl)) {
			if (Input.GetKeyDown (KeyCode.S))
				SavePlant ();

			if (Input.GetKeyDown (KeyCode.O))
				OpenPlant ();

			if (Input.GetKeyDown (KeyCode.N))
				NewPlant ();

			if (Input.GetKeyDown (KeyCode.Q))
				Quit ();
		}
	}


	public void ToggleMenu()
	{
		opened = !opened;
		filePanel.SetActive (opened);




	}
		

	public void SavePlant()
	{
		Debug.Log ("Save Plant");
		plantSerializer.SerializePlant (plantPartPanelManager, plantPartPanelManager.targetedPlant.name);
	}

	public void OpenPlant()
	{
		Debug.Log ("Open Plant");
		ToggleMenu ();

		openPanel.gameObject.SetActive (true);
		openPanel.Open ();
	}

	public void NewPlant()
	{
		Debug.Log ("New Plant");

		ToggleMenu ();

	}

	public void Quit()
	{
		Debug.Log ("Quit");





	}



}
