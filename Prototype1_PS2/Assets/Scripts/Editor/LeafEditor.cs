using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Leaf))]
public class LeafEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Leaf myObject = (Leaf)target;


		EditorGUI.BeginChangeCheck ();

		base.OnInspectorGUI ();

		if (EditorGUI.EndChangeCheck ())
			myObject.Initialize ();
		
		if (GUILayout.Button("Update", GUILayout.Height(32f)))
			myObject.Initialize ();


	}
}
