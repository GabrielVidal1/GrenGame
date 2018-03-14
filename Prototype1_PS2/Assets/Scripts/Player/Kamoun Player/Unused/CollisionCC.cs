using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCC : MonoBehaviour {

	// Use this for initialization
	private void OnCollisionEnter(Collision col)
	{
		Debug.Log(col.gameObject.name);
	}

	
	

}
