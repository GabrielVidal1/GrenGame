using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct PlantInformation {

	public string plantName;

	public string plantLatinName;

	public string plantDescription;

	public Texture plantTexture;

	public PlantInformation(string name)
	{
		plantName = name;
		plantLatinName = name + "us";
		plantDescription = "";
		plantTexture = null;
	}
}
