using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WorldButton : MonoBehaviour {

	public TMP_Text worldNameText;

	private string worldName;

	void Start () 
	{
		



	}

	public void Initialize(string wd)
	{
		worldName = wd;
		worldNameText.text = wd;

	}

	public void LauchWorld()
	{

		GameManager.gm.SetWorld (worldName);



	}

}
