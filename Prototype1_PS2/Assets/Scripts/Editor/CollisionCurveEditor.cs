#if UNITY_EDITOR
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CollisionCurve))]
public class CollisionCurveEditor :  Editor{

	public override void OnInspectorGUI ()
	{
		CollisionCurve myObject = (CollisionCurve)target;

		EditorGUI.BeginChangeCheck ();
		base.OnInspectorGUI ();

		if (EditorGUI.EndChangeCheck ())
			myObject.GetComponent<PlantGeneration> ().Initialize ();

	}
}
#endif