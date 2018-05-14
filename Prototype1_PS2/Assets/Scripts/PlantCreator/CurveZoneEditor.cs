#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurveZone))]
public class CurveZoneEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		CurveZone myObject = (CurveZone)target;


		if (GUILayout.Button ("Save Texture")) {
			myObject.SaveTexture ();
		}
	}

}
#endif