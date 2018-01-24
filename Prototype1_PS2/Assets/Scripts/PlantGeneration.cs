using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGeneration : MonoBehaviour {

	public float initialRadius;
	public Vector3 initalDirection;

	public int nbOfSides;

	public AnimationCurve radiusOverLength;


	public List<Vector3> points;

	public List<Vector3> faces;




	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	public void GenerateMesh()
	{
		Vector3 u = Vector3.Dot (initalDirection, Vector3.up);
		Vector3 v = Vector3.Dot (initalDirection, u);

		Vector3 pos = Vector3.zero;

		for (int i = 0; i < nbOfSides; i++) {





		}





	}
*/
}
