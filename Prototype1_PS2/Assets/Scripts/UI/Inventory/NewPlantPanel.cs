using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NewPlantPanel : MonoBehaviour {

	public TMP_InputField newPlantName;
	public RawImage plantIcon1, plantIcon2;

	[SerializeField] private GeneticCrossingPanel geneticCrossingPanel;

	private bool tfp1;

	public void Init()
	{


		ChoseIcon1 ();
	}

	public void ChoseIcon1()
	{
		tfp1 = true;
		plantIcon1.transform.parent.GetComponent<RawImage> ().enabled = true;
		plantIcon2.transform.parent.GetComponent<RawImage> ().enabled = false;
	}

	public void ChoseIcon2()
	{
		tfp1 = false;
		plantIcon2.transform.parent.GetComponent<RawImage> ().enabled = true;
		plantIcon1.transform.parent.GetComponent<RawImage> ().enabled = false;
	}

	public void Create()
	{
		string plantName = newPlantName.text;

		geneticCrossingPanel.CreateCrossedPlant (tfp1, plantName);

		gameObject.SetActive (false);

	}
}
