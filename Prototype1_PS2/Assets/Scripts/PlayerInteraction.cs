using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	public GameObject camera;
	public GameObject tree;

	void Start () {
		
	}
	
	void Update () 
	{


		if (Input.GetMouseButtonDown (0)) {

			Ray ray = new Ray (camera.transform.position + camera.transform.forward, camera.transform.forward);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 1000f)) {
				Instantiate (tree, hit.point, Quaternion.identity);

				//tree.GetComponent<PlantGeneration> ().initalDirection = hit.normal;
				tree.GetComponent<Plant> ().initalDirection = hit.normal;

			}




		}

		
	}
}
