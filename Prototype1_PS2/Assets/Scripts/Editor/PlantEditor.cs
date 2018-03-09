using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Plant))]
public class PlantEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Plant myObject = (Plant)target;

		EditorGUI.BeginChangeCheck ();


		base.OnInspectorGUI ();

		if (EditorGUI.EndChangeCheck ()) {
			myObject.Initialize ();
		}

		if (GUILayout.Button("UPDATE", GUILayout.Height(32f)))
			myObject.Initialize ();


	}
}
