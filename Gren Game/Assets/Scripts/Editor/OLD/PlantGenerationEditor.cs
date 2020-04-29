#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlantGeneration))]
public class PlantGenerationEditor : Editor {


	private bool hasBeenInitialized = false;

	public override void OnInspectorGUI ()
	{
		PlantGeneration myObject = (PlantGeneration)target;


		EditorGUI.BeginChangeCheck ();
		base.OnInspectorGUI ();

		if (EditorGUI.EndChangeCheck())
			myObject.Initialize ();


		if (GUILayout.Button("Force Initialization"))
			myObject.Initialize ();




		if (GUILayout.Button ("UpdateMesh")) {
			if (!hasBeenInitialized) {
				myObject.Initialize ();
				hasBeenInitialized = true;
			} else {
				myObject.UpdateMesh ();
			}
		}




	}
}
#endif