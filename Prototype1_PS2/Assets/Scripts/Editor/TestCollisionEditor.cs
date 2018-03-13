#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestCollision))]
public class TestCollisionEditor :  Editor{

	public override void OnInspectorGUI ()
	{
		TestCollision myObject = (TestCollision)target;

		EditorGUI.BeginChangeCheck ();
		base.OnInspectorGUI ();

		if (EditorGUI.EndChangeCheck ())
			myObject.GetComponent<PlantGeneration> ().Initialize ();




	}
}
#endif