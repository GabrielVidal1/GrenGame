using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour {

	[Range(0f, 1f)]
	public float growthSpeed;

	[Range(1, 24)]
	public int updateRate;

	Plant pg;
	private float lastUpdate;

	void Start()
	{
		pg = GetComponent<Plant> ();
		pg.time = 0f;
		pg.Initialize ();
		lastUpdate = Time.time;
	}

	void Update () 
	{
		if (pg.time <= 1f) {



			pg.time += Time.deltaTime * growthSpeed;

			if (updateRate * Time.deltaTime + lastUpdate <= Time.time) {	
				pg.UpdateMesh ();
				lastUpdate = Time.time;
			}
		}

	}
}
