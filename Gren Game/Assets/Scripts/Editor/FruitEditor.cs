using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Fruit))]
public class FruitEditor : Editor {

	public override void OnInspectorGUI ()
	{
		EditorGUI.BeginChangeCheck ();

		DrawDefaultInspector ();


		Fruit myObject = (Fruit)target;


		if (EditorGUI.EndChangeCheck())
			myObject.Initialize ();



		if (GUILayout.Button ("Initialize", GUILayout.Height (32f)))
			myObject.Initialize ();

	}
}
