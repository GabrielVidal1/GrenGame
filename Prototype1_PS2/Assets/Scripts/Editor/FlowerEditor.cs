using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Flower))]
public class FlowerEditor : Editor {

	public override void OnInspectorGUI ()
	{
		EditorGUI.BeginChangeCheck ();

		DrawDefaultInspector ();


		Flower myObject = (Flower)target;


		if (EditorGUI.EndChangeCheck())
			myObject.Initialize ();



		if (GUILayout.Button ("Initialize", GUILayout.Height (32f)))
			myObject.Initialize ();

	}
}
