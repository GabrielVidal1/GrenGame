using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NewPlantPanel : MonoBehaviour {

	public TMP_InputField newPlantName;
	public RawImage plantIcon1, plantIcon2;

	public Texture newPlantTexture;

	public void Init()
	{


		ChoseIcon1 ();
	}

	public void ChoseIcon1()
	{
		newPlantTexture = plantIcon1.texture;
		plantIcon1.transform.parent.GetComponent<RawImage> ().enabled = true;
		plantIcon2.transform.parent.GetComponent<RawImage> ().enabled = false;
	}

	public void ChoseIcon2()
	{
		newPlantTexture = plantIcon2.texture;
		plantIcon2.transform.parent.GetComponent<RawImage> ().enabled = true;
		plantIcon1.transform.parent.GetComponent<RawImage> ().enabled = false;
	}

	public void Create()
	{






	}
}
