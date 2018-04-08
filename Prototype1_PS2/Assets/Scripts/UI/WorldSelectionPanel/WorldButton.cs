using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WorldButton : MonoBehaviour {

	public TMP_Text worldNameText;


	//[SerializeField] private WorldSelectionPanel worldSelectionPanel;

	private string worldName;
	void Start () 
	{
		



	}

	public void Initialize(string wn)
	{
		worldName = wn;
		worldNameText.text = wn;

	}

	public void LauchWorld()
	{

		Debug.Log(transform.parent.parent.parent.parent.parent.name);
		transform.parent.parent.parent.parent.parent.GetComponent<WorldSelectionPanel> ().Launch (worldName);
	}

}
