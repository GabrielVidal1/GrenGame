using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerInteract : NetworkBehaviour {

	private Player player;
	private GameObject camera;

	void Start () 
	{
		player = GetComponent<Player> ();
		camera = player.camera;
	}


	[Command]
	public void CmdInteract(Vector3 origin, Vector3 direction)
	{
		RpcInteract ( origin, direction);
	}

	[ClientRpc]
	public void RpcInteract(Vector3 origin, Vector3 direction)
	{


		Ray ray = new Ray (origin, direction);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100f)) {
			if (hit.collider.tag == "Door") {

				Door door = hit.collider.GetComponent<Door> ();

				door.Interact ();


			} else 
				Debug.LogError ("This zone is impossible to plant on !!");
		}
	}



	void Update () 
	{
		if (!isLocalPlayer)
			return;


		if (!CanvasManager.cm.inGameMenu.isPaused) {

			//PLANT SEED
			if (Input.GetKeyDown(KeyCode.E)) {

				Vector3 origin = camera.transform.position;
				Vector3 direction = camera.transform.forward;

				Ray ray = new Ray (origin, direction);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, 100f)) {

					Debug.Log ("hit !! ");

					if (hit.collider.tag == "Door") {
						Debug.Log ("hit DOOR !! ");

						CmdInteract (camera.transform.position, camera.transform.forward);
					}
				}


			}
		}

	}
}
