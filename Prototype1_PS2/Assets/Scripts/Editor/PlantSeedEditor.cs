#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlantSeed))]
public class PlantSeedEditor : Editor {

	PlantManager pm;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnInspectorGUI ()
	{
		if (pm == null)
			pm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PlantManager> ();

		PlantSeed myObject = (PlantSeed)target; 

		base.OnInspectorGUI ();

		myObject.indexInPlantManager = EditorGUILayout.IntSlider ("Index in Plant Array", myObject.indexInPlantManager,
			0, pm.plantsPrefabs.Length - 1);




		EditorGUILayout.LabelField ("Plant Type", pm.plantsPrefabs [myObject.indexInPlantManager].name);

	}
}
#endif