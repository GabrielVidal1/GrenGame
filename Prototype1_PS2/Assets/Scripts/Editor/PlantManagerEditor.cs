#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlantManager))]
public class PlantManagerEditor : Editor {

	public override void OnInspectorGUI ()
	{
		PlantManager myObject = (PlantManager)target;

		DrawDefaultInspector ();

		if (GUILayout.Button ("Attribute Plant Indexes")) {
			myObject.AttributePlantIndexes ();
		}
	}

}
#endif