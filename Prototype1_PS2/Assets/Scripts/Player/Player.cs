using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> {}

public class PlayerMessage : MessageBase
{
	//public NetworkInstanceId netId;

	public PlantSave[] plantsToSync;

	public PlayerMessageAction pma;

}


public class Player : NetworkBehaviour {

	[SerializeField] ToggleEvent onToggleLocal;
	[SerializeField] ToggleEvent onToggleShared;
	[SerializeField] ToggleEvent onToggleRemote;

	//private NetworkIdentity ni;

	public override void OnStartClient ()
	{
		GameManager.gm.nm.client.RegisterHandler(1000, OnMessageRecieve);
	}



	void Start()
	{
		EnablePlayer ();
	}


	public void DisablePlayer()
	{
		GameManager.gm.localPlayer = null;


		onToggleShared.Invoke (false);

		if (isLocalPlayer)
			onToggleLocal.Invoke (false);
		else
			onToggleRemote.Invoke (false);
	}

	void EnablePlayer()
	{
		//ni = GetComponent<NetworkIdentity> ();
		GameManager.gm.localPlayer = this;

		onToggleShared.Invoke (true);

		if (isLocalPlayer)
			onToggleLocal.Invoke (true);
		else
			onToggleRemote.Invoke (true);


		//IF CLIENT AND NOT HOST
		if (isClient && !GameManager.gm.isHost) {
			//Debug.Log ("Plants are being uploaded...");
			CmdSynchronisePlants ();
		}
	}

	[Command]
	public void CmdSynchronisePlants()
	{
		PlayerMessage pm = new PlayerMessage ();

		//GET THE COMPLETE VERSION OF PLANTS ON THE HOST PLAYER GAMEDATA
		pm.plantsToSync = GameManager.gm.GetPlantArrayToTransmit ();

		//SET THE MESSAGE TYPE TO SYNC THE PLANTS
		pm.pma = PlayerMessageAction.SynchronisePlants;

		//SEND THE MESSAGE
		base.connectionToClient.Send (1000, pm);
	}

	static void OnMessageRecieve(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<PlayerMessage> ();

		//IF NEED TO SYNC PLANTS
		if (msg.pma == PlayerMessageAction.SynchronisePlants) {
			//Debug.Log ("local unity loads plants");
			GameManager.gm.LoadPlants (msg.plantsToSync);
		}
	}
}

public enum PlayerMessageAction
{
	SynchronisePlants,
	None
}