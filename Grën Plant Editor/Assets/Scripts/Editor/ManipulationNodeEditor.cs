#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ManipulationNode))]
public class ManipulationNodeEditor : Editor {

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();

		ManipulationNode myObject = (ManipulationNode)target;

		EditorGUILayout.LabelField ("AXES");

		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("", GUILayout.Width(Screen.width / 4f));
		EditorGUILayout.LabelField ("X", GUILayout.Width(Screen.width / 4f));
		EditorGUILayout.LabelField ("Y", GUILayout.Width(Screen.width / 4f));
		EditorGUILayout.LabelField ("Z", GUILayout.Width(Screen.width / 4f));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Toggle Axis", GUILayout.Width(Screen.width / 4f));

		myObject.xAxis = EditorGUILayout.Toggle (GUIContent.none, myObject.xAxis, GUILayout.Width(Screen.width / 4f));
		myObject.yAxis = EditorGUILayout.Toggle (GUIContent.none, myObject.yAxis, GUILayout.Width(Screen.width / 4f));
		myObject.zAxis = EditorGUILayout.Toggle (GUIContent.none, myObject.zAxis, GUILayout.Width(Screen.width / 4f));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Min", GUILayout.Width(Screen.width / 4f));

		myObject.xRange.x = EditorGUILayout.FloatField (GUIContent.none, myObject.xRange.x, GUILayout.Width(Screen.width / 4f));
		myObject.yRange.x = EditorGUILayout.FloatField (GUIContent.none, myObject.yRange.x, GUILayout.Width(Screen.width / 4f));
		myObject.zRange.x = EditorGUILayout.FloatField (GUIContent.none, myObject.zRange.x, GUILayout.Width(Screen.width / 4f));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Max", GUILayout.Width(Screen.width / 4f));

		myObject.xRange.y = EditorGUILayout.FloatField (GUIContent.none, myObject.xRange.y, GUILayout.Width(Screen.width / 4f));
		myObject.yRange.y = EditorGUILayout.FloatField (GUIContent.none, myObject.yRange.y, GUILayout.Width(Screen.width / 4f));
		myObject.zRange.y = EditorGUILayout.FloatField (GUIContent.none, myObject.zRange.y, GUILayout.Width(Screen.width / 4f));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Ratio", GUILayout.Width(Screen.width / 4f));

		myObject.xRatio = EditorGUILayout.FloatField (GUIContent.none, myObject.xRatio, GUILayout.Width(Screen.width / 4f));
		myObject.yRatio = EditorGUILayout.FloatField (GUIContent.none, myObject.yRatio, GUILayout.Width(Screen.width / 4f));
		myObject.zRatio = EditorGUILayout.FloatField (GUIContent.none, myObject.zRatio, GUILayout.Width(Screen.width / 4f));
		GUILayout.EndHorizontal ();

		EditorGUILayout.Space ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Value", GUILayout.Width(Screen.width / 4f));
		EditorGUILayout.LabelField (myObject.xValue.ToString(), GUILayout.Width(Screen.width / 4f));
		EditorGUILayout.LabelField (myObject.yValue.ToString(), GUILayout.Width(Screen.width / 4f));
		EditorGUILayout.LabelField (myObject.zValue.ToString(), GUILayout.Width(Screen.width / 4f));
		GUILayout.EndHorizontal ();
	}
}
#endif