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

	public WorldData wd;

	public PlayerMessageAction pma;

}


public class Player : NetworkBehaviour {

	[SerializeField] ToggleEvent onToggleLocal;
	[SerializeField] ToggleEvent onToggleShared;
	[SerializeField] ToggleEvent onToggleRemote;

	public GameObject camera;

	public string playerName;

	public bool synched;

	public override void OnStartClient ()
	{
		GameManager.gm.nm.client.RegisterHandler(1000, OnMessageRecieve);
	}



	void Start()
	{
		EnablePlayer ();

	}

	[Command]
	public void CmdSetNameOnServer(string name)
	{
		//Debug.Log ("my previous name was : " + playerName+". Now, it is "+ name);
		playerName = name;
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
		synched = false;

		onToggleShared.Invoke (true);


		//Debug.Log ("je suis le joueur et je suis " + playerName);

		if (isLocalPlayer) {
			onToggleLocal.Invoke (true);

			GameManager.gm.localPlayer = this;
			playerName = GameManager.gm.GetPlayerName ();
			CmdSetNameOnServer (playerName);



			if (isClient && !GameManager.gm.isHost) {
				//Debug.Log ("Worlds are being synchronized...");
				CmdSynchroniseWorld ();
			} else {
				
				GameManager.gm.wd.LoadPlayerInformation (this);
			}

		} else
			onToggleRemote.Invoke (true);
	}


	[Command]
	public void CmdSynchroniseWorld()
	{
		PlayerMessage pm = new PlayerMessage ();

		//GET THE COMPLETE VERSION OF WORLD ON THE HOST PLAYER WorldSerialization

		//Debug.Log ("je serialise le monde chez l'host");

		GameManager.gm.wd.SerializeWorld();
		pm.wd = GameManager.gm.wd.worldData;

		//(new GameObject ("TEST WORLDATA DEFORE SENDING")).AddComponent<Test> ().wd = pm.wd;


		//SET THE MESSAGE TYPE TO SYNC THE PLANTS
		pm.pma = PlayerMessageAction.LoadWorld;

		//SEND THE MESSAGE
		base.connectionToClient.Send (1000, pm);
	}

	static void OnMessageRecieve(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<PlayerMessage> ();

		//IF NEED TO SYNC World
		if (msg.pma == PlayerMessageAction.LoadWorld) {

			//(new GameObject ("TEST WORLDATA AFTER RECEIVING")).AddComponent<Test> ().wd = msg.wd;

			//GameManager.gm.wd.worldData = msg.wd;


			GameManager.gm.wd.DeserializeWorld (msg.wd);
			//Debug.Log ("je deserialize le monde");
		}
	}
}

public enum PlayerMessageAction
{
	LoadWorld,
	None
}