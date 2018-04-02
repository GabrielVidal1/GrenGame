using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlayButton : MonoBehaviour {

	public void LaunchWorld()
	{
		Debug.Log ("Lauch World!");
		transform.parent.GetComponent<WorldButton> ().LauchWorld ();
	}
}
