using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerPlanter : NetworkBehaviour {


	private GameObject camera;
	private Player player;
	private PlayerInventory playerInventory;

	private int layerMask;

	void Start () 
	{
		layerMask = ~(1 << 1);


		player = GetComponent<Player> ();
		camera = player.camera;
		playerInventory = GetComponent<PlayerInventory> ();
	}


	[Command]
	public void CmdPlant(int index, Vector3 position, Vector3 direction)
	{
		RpcPlant (index, position, direction);
	}

	[ClientRpc]
	public void RpcPlant(int index, Vector3 position, Vector3 direction)
	{
		Plant plant = Instantiate (GameManager.gm.pm.plantsPrefabs [index], position, Quaternion.identity);
		plant.initialDirection = direction;
		plant.SetSeed (Plant.plantNumber);

		plant.time = 0f;
		plant.InitializePlant ();

		GameManager.gm.pm.plants.Add (plant);
	}



	void Update () 
	{
		if (!isLocalPlayer)
			return;


		if (!CanvasManager.cm.inGameMenu.isPaused) {

			//PLANT SEED
			if (Input.GetMouseButtonDown (0)) {


				if (playerInventory.NbOfSeeds > 0) {

					if (CanvasManager.cm.seedSelectionWheel.canClick) {

						Ray ray = new Ray (camera.transform.position /*+ camera.transform.forward*/, camera.transform.forward);
						RaycastHit hit;

						if (Physics.Raycast (ray, out hit, 100f, layerMask)) {



							Vector3 dir = hit.normal;

							CmdPlant (playerInventory.PlantIndex, hit.point + 0.1f * hit.normal, hit.normal);
							playerInventory.UseSeed ();

							//Debug.DrawRay (hit.point, hit.normal, Color.red, 10f);
						}
					}
				}
			}
			/*
			if (Input.GetKeyDown (KeyCode.E)) {
				selectedIndex = (selectedIndex + 1) % GameManager.gm.pm.plantsPrefabs.Length;
				UpdateSelectedPlantText ();
			}
			*/
		}
		
	}
}
