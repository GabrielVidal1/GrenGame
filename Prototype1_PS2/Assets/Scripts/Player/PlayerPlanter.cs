using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerPlanter : NetworkBehaviour {

	public GameObject camera;

	int selectedIndex = 0;

	void Start () 
	{
		UpdateSelectedPlantText ();
	}


	[Command]
	public void CmdPlant(int index, Vector3 position, Vector3 direction)
	{
		RpcPlant (index, position, direction);
	}

	[ClientRpc]
	public void RpcPlant(int index, Vector3 position, Vector3 direction)
	{
		GameManager.gm.CreateNewPlant (index, 0f, position, direction, Plant.plantNumber);
	}
	
	void Update () 
	{
		if (!isLocalPlayer)
			return;


		if (!CanvasManager.cm.inGameMenu.isPaused) {
			if (Input.GetMouseButtonDown (0)) {

				Ray ray = new Ray (camera.transform.position + camera.transform.forward, camera.transform.forward);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, 1000f)) {

					Vector3 dir = 1.1f * hit.normal;

					CmdPlant (selectedIndex, hit.point + 0.05f * hit.normal, hit.normal);
					//Debug.DrawRay (hit.point, hit.normal, Color.red, 10f);
				}
			}


			if (Input.GetKeyDown (KeyCode.E)) {
				selectedIndex = (selectedIndex + 1) % GameManager.gm.pm.plantsPrefabs.Length;
				UpdateSelectedPlantText ();
			}
		}
		
	}


	void UpdateSelectedPlantText()
	{
		CanvasManager.cm.inGameMenu.selectedPlant.text = "Selected Plant (" + selectedIndex.ToString ()+"): " + GameManager.gm.pm.plantsPrefabs [selectedIndex].gameObject.name;
	}
}
