using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenPanelSlot : MonoBehaviour {

	[SerializeField] private TMP_Text plantName;


	string path;
	OpenPanel openPanel;

	public void Initialize(OpenPanel openPanel, string path, string plantName)
	{
		this.plantName.text = plantName;
		this.openPanel = openPanel;
		this.path = path;
	}

	public void LoadPlant()
	{
		openPanel.LoadPlant (path, plantName.text);
	}
}
