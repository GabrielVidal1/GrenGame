using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerPlanter : NetworkBehaviour {

	public float reach = 100f;

	private GameObject camera;
	private Player player;
	private PlayerInventory playerInventory;

	private int layerMask;

	void Start () 
	{
		layerMask = /*~(1 << 1);*/ LayerMask.GetMask ("FertileGround");


		player = GetComponent<Player> ();
		camera = player.camera;
		playerInventory = GetComponent<PlayerInventory> ();
	}


	[Command]
	public void CmdPlant(int index, Vector3 origin, Vector3 direction)
	{
		RpcPlant (index, origin, direction);
	}

	[ClientRpc]
	public void RpcPlant(int index, Vector3 origin, Vector3 direction)
	{


		Ray ray = new Ray (origin, direction);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100f)) {
			if (hit.collider.tag == "Zone") {


				Plant plant = Instantiate (GameManager.gm.pm.plantsPrefabs [index], hit.point + 0.1f * hit.normal, Quaternion.identity);
				plant.InitialDirection = hit.normal;
				plant.SetSeed (Plant.plantNumber);
			
				plant.time = 0f;
				plant.InitializePlant ();

				plant.GetComponent<TreeGrowth> ().Lauch ();
			
				playerInventory.UseSeed ();


				Zone touchedZone = hit.collider.GetComponent<Zone> ();

				int plantIndex = GameManager.gm.pm.AddPlantAndGetIndex (plant);

				if (touchedZone) {
					touchedZone.AddPlant (plantIndex);
				}

				return;
			}

			
			Debug.LogError ("This zone is impossible to plant on !!");



		}
	}



	void Update () 
	{
		if (!isLocalPlayer)
			return;


		if (!CanvasManager.cm.inGameMenu.isPaused &&
			!CanvasManager.cm.playerInventoryGrid.Opened) {

			//PLANT SEED
			if (Input.GetMouseButtonDown (0)) {


				if (playerInventory.NbOfSeeds > 0) {

					if (CanvasManager.cm.seedSelectionWheel.canClick) {

						Vector3 origin = camera.transform.position;
						Vector3 direction = camera.transform.forward;

						Ray ray = new Ray (origin, direction);
						RaycastHit hit;

						if (Physics.Raycast (ray, out hit, 100f)) {
							if (hit.collider.tag == "Zone")
								CmdPlant (playerInventory.PlantIndex,origin, direction);
						}



					}
				}
			}
		}
		
	}
}
