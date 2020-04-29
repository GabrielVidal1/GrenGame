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
			Object[] zones = GameObject.FindGameObjectsWithTag ("Zone");
			if (myObject.zones != null)
				myObject.zones.Clear ();
			else
				myObject.zones = new List<Zone> ();

			for (int i = 0; i < zones.Length; i++) {
				myObject.zones.Add (((GameObject)zones[i]).GetComponent<Zone> ());
			}

			Object[] doors = GameObject.FindGameObjectsWithTag ("Door");
			if (myObject.doors != null)
				myObject.doors.Clear ();
			else
				myObject.doors = new List<Door> ();

			for (int i = 0; i < doors.Length; i++) {
				myObject.doors.Add (((GameObject)doors[i]).GetComponent<Door> ());
			}
			//myObject.doors = new List<Zone> (GameObject.FindObjectsOfType (typeof(Door)));



		}
	}
}
#endif