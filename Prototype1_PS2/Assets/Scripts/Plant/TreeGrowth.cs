using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour {

	[Range(0f, 1f)]
	public float growthSpeed;

	[Range(1, 24)]
	public int updateRate;

	//public bool canGrow;

	[Range(0.01f, 5f)]
	public float mossRadius;

	Plant pg;
	private float lastUpdate;

	private Moss moss;

	void Start()
	{
		//canGrow = true;
		pg = GetComponent<Plant> ();

		pg.InitializePlant ();

		growthSpeed = 1f / pg.maxDuration;
		lastUpdate = Time.time;

		moss = (Moss)Instantiate (GameManager.gm.pm.mossPrefab, transform);

		moss.finalRadius = mossRadius;
	}

	void Update () 
	{
		//if (canGrow) {
		if (pg.time < 1f) {



			pg.time += Time.deltaTime * growthSpeed;

			if (updateRate * Time.deltaTime + lastUpdate <= Time.time) {

				moss.time = pg.time;
				moss.UpdateMoss ();
				//GameManager.SavePlantTime (pg.indexInGameData, pg.time);

				pg.UpdatePlant ();
				lastUpdate = Time.time;
			}
		} else {
			pg.time = 1f;

		}
	}
}
