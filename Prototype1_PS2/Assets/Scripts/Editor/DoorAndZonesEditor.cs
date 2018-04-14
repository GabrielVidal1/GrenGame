#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ZoneAndDoorManager))]
public class DoorAndZonesEditor : Editor {

	public override void OnInspectorGUI ()
	{
		ZoneAndDoorManager myObject = (ZoneAndDoorManager)target;

		DrawDefaultInspector ();

		if (GUILayout.Button ("Update")) {
			Object[] t = GameObject.FindObjectsOfType (typeof(Zone));
			if (myObject.zones != null)
				myObject.zones.Clear ();
			else
				myObject.zones = new List<Zone> ();

			for (int i = 0; i < t.Length; i++) {
				myObject.zones.Add (((GameObject)t[i]).GetComponent<Zone> ());
			}


			//myObject.doors = new List<Zone> (GameObject.FindObjectsOfType (typeof(Door)));



		}
	}
}
#endif