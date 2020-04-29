using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAndDoorManager : MonoBehaviour {


	public List<Zone> zones;

	public List<Door> doors;

	public void Init()
	{
		Object[] z = GameObject.FindGameObjectsWithTag ("Zone");
		if (zones != null)
			zones.Clear ();
		else
			zones = new List<Zone> ();

		for (int i = 0; i < z.Length; i++) {
			zones.Add (((GameObject)z[i]).GetComponent<Zone> ());
		}

		Object[] d = GameObject.FindGameObjectsWithTag ("Door");
		if (doors != null)
			doors.Clear ();
		else
			doors = new List<Door> ();

		for (int i = 0; i < d.Length; i++) {
			doors.Add (((GameObject)d[i]).GetComponent<Door> ());
		}


	}


	public void SerializeZones(WorldData wd)
	{
		wd.zones = new SerializedZone[zones.Count];
		for (int i = 0; i < zones.Count; i++) {
			wd.zones [i] = new SerializedZone (zones [i]);
		}

	}

	public void SerializeDoors(WorldData wd)
	{
		wd.doors = new SerializedDoor[doors.Count];
		for (int i = 0; i < doors.Count; i++) {
			wd.doors [i] = new SerializedDoor (doors [i]);
		}
	}

	public void DeserializeZones(WorldData wd)
	{
		for (int i = 0; i < wd.zones.Length; i++) {
			zones [i].nbOfPoints = wd.zones [i].nbOfPoints;
		}
	}

	public void DeserializeDoors(WorldData wd)
	{
		for (int i = 0; i < wd.doors.Length; i++) {
			if (wd.doors [i].open) {
				doors [i].InitOpen ();
			}
			doors [i].UpdateSlider ();
		}
	}
}
