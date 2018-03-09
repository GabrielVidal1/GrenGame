using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour {

	public float growthSpeed;

	Plant pg;

	void Start()
	{
		pg = GetComponent<Plant> ();
		pg.time = 0f;
		pg.Initialize ();
	}

	void Update () 
	{
		if (pg.time < 1f) {
			pg.time += Time.deltaTime * growthSpeed;
			pg.UpdateMesh ();
		}

	}
}
