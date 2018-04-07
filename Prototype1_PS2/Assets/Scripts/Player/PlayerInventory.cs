using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour {

	public List<PlantSeedInventory> inventory;

	public int NbOfSeeds
	{
		get { return inventory.Count; }
	}

	public float reachDistance;


	private int selectedIndexInInventory = 0;

	private int layerMask;


	public int PlantIndex {
		
		get {Debug.Log (selectedIndexInInventory + "/" + inventory.Count);
			return inventory [selectedIndexInInventory].plantSeedIndexInPlantManager; }
	}


	#region Private Variables
	private Player player;
	private GameObject camera;
	#endregion

	void Start () 
	{
		if (isLocalPlayer) {
			CanvasManager.cm.seedSelectionWheel.SetPlayer (this);
		}

		layerMask = ~(1 << 1);
	
		player = GetComponent<Player> ();
		camera = player.camera;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isLocalPlayer)
			return;

		if (!CanvasManager.cm.inGameMenu.isPaused) {

			if (Input.GetKeyDown (KeyCode.E)) {
				Ray ray = new Ray (camera.transform.position, camera.transform.forward);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, reachDistance, layerMask)) {

					if (hit.collider.tag == "Pickup") {
						CmdRayCastPickup (ray.origin, ray.direction);
					}
				}
			}

			float mouseScroll = Input.mouseScrollDelta.y;
			if (mouseScroll != 0) {
				if (NbOfSeeds > 1) {
					if (mouseScroll < 0) {
						selectedIndexInInventory = (selectedIndexInInventory + 1) % NbOfSeeds; 
					} else {
						selectedIndexInInventory = ((selectedIndexInInventory - 1) + NbOfSeeds) % NbOfSeeds;
					}
					CanvasManager.cm.seedSelectionWheel.SetNewSelectedPlantIndex (selectedIndexInInventory);
				}
			}
		}
	}




	public void UseSeed()
	{
		inventory [selectedIndexInInventory] = 
			new PlantSeedInventory (inventory [selectedIndexInInventory].plantSeedIndexInPlantManager, 
				inventory [selectedIndexInInventory].number - 1);


		if (inventory [selectedIndexInInventory].number == 0) {
			inventory.RemoveAt (selectedIndexInInventory);
			selectedIndexInInventory = Mathf.Min (selectedIndexInInventory, NbOfSeeds - 1);
			if (isLocalPlayer)
				CanvasManager.cm.seedSelectionWheel.UseSeed (selectedIndexInInventory, true);
		} else {
			if (isLocalPlayer)
				CanvasManager.cm.seedSelectionWheel.UseSeed (selectedIndexInInventory, false);
		}

	}


	public void AddSeedToInventory(PlantSeed seed)
	{

		if (NbOfSeeds == 0)
			selectedIndexInInventory = 0;

		int index = -1;

		for (int i = 0; i < inventory.Count; i++) {
			if (inventory [i].plantSeedIndexInPlantManager == seed.indexInPlantManager) {
				
				index = i;
				inventory [i] = new PlantSeedInventory (inventory [i].plantSeedIndexInPlantManager, inventory [i].number + 1);
				print (inventory [i].number);
				//print ("found a slot :" + i);
			}
		}
		if (index == -1) {
			//print ("new slot");
			inventory.Add (new PlantSeedInventory(seed.indexInPlantManager, 1));
		}


		if (isLocalPlayer)
			CanvasManager.cm.seedSelectionWheel.AddSeed (index);
	}




	[Command]
	void CmdRayCastPickup(Vector3 position, Vector3 direction)
	{
		RpcRayCastPickup (position, direction);
	}

	[ClientRpc]
	void RpcRayCastPickup(Vector3 position, Vector3 direction)
	{
		Ray ray = new Ray (position, direction);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, reachDistance, layerMask)) {

			if (hit.collider.tag == "Pickup") {

				Pickup p = hit.collider.GetComponent<Pickup> ();

				p.PickupItem (this);

				return;
			}
		}

		Debug.Log ("Impossible to reach this part of the code => Multiplayer Sync Problems");
	}
}

[System.Serializable]
public struct PlantSeedInventory
{
	public int plantSeedIndexInPlantManager;
	public int number;

	public PlantSeedInventory(int plantSeedIndexInPlantManager, int number)
	{
		this.plantSeedIndexInPlantManager = plantSeedIndexInPlantManager;
		this.number = number;
	}

}