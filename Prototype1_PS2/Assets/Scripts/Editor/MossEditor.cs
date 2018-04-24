#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Moss))]
public class MossEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Moss myObject = (Moss)target;

		EditorGUI.BeginChangeCheck ();
		DrawDefaultInspector ();
		if (EditorGUI.EndChangeCheck ()) {
			myObject.UpdateMoss ();
		}

	}
}
#endif