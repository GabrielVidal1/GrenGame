#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlantEditorGUI))]
public class PlantEditorGUIEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		PlantEditorGUI myObject = (PlantEditorGUI)target;

	}
}
#endif