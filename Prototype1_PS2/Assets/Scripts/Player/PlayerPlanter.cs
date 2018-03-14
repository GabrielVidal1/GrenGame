using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerPlanter : NetworkBehaviour {

	public GameObject camera;
	public GameObject[] plants;

	int selectedIndex = 0;

	void Start () 
	{
		CanvasManager.cm.inGameMenu.selectedPlant.text = "Selected Plant : " + plants [selectedIndex].name;

	}


	[Command]
	public void CmdPlant(int index, Vector3 position, Vector3 direction)
	{
		RpcPlant (index, position, direction);
	}


	[ClientRpc]
	public void RpcPlant(int index, Vector3 position, Vector3 direction)
	{

		Plant plant = Instantiate (plants [index], position, Quaternion.identity).GetComponent<Plant>();
		plant.initalDirection = direction;
		plant.time = 0f;

	}

	
	void Update () 
	{
		if (!isLocalPlayer)
			return;



		if (Input.GetMouseButtonDown (0)) {

			Ray ray = new Ray (camera.transform.position + camera.transform.forward, camera.transform.forward);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 1000f)) 
			{

				Vector3 dir = 1.1f * hit.normal;

				CmdPlant (selectedIndex, hit.point + 0.05f * hit.normal, hit.normal);
				//Debug.DrawRay (hit.point, hit.normal, Color.red, 10f);
			}
		}


		if (Input.GetKeyDown (KeyCode.E)) {
			selectedIndex = (selectedIndex + 1) % plants.Length;
			CanvasManager.cm.inGameMenu.selectedPlant.text = "Selected Plant : " + plants [selectedIndex].name;
		}

		
	}
}
