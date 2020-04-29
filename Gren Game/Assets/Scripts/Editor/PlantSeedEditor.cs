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

		//PlantSeed myObject = (PlantSeed)target; 

		base.OnInspectorGUI ();


		serializedObject.FindProperty("indexInPlantManager").intValue = EditorGUILayout.IntSlider ("Index in Plant Array", serializedObject.FindProperty("indexInPlantManager").intValue,
			0, pm.plantsPrefabs.Count - 1);


		serializedObject.ApplyModifiedProperties ();

		EditorGUILayout.LabelField ("Plant Type", pm.plantsPrefabs [serializedObject.FindProperty("indexInPlantManager").intValue].name);

	}
}
#endif