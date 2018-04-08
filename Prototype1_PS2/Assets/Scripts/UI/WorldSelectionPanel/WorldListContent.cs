using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WorldListContent : MonoBehaviour {


	public void SetHeight(int nbOfWorlds, float worldHeigth)
	{

		RectTransform rTransform = GetComponent<RectTransform> ();

		float heigth = (10f + worldHeigth) * nbOfWorlds + 20f;

		print ("heigth : " + heigth);

		rTransform.sizeDelta = new Vector2 (rTransform.sizeDelta.x,heigth);
		//rTransform.pivot = new Vector2 (rTransform.pivot.x, 0); 
	}




}
