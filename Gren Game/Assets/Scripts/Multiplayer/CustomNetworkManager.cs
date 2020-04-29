using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class CustomNetworkManager : NetworkManager {

	public override void OnClientConnect (NetworkConnection conn)
	{
		Debug.Log ("Un client a rejoin");

		GameManager.gm.clientConnection = conn;

		if (GameManager.gm.isHost) {

			base.OnClientConnect (conn);


		}
	}

	public override void OnServerDisconnect (NetworkConnection conn)
	{
		
		GameManager.gm.Save ();

		Debug.Log ("Player Disconnection !");
		NetworkServer.DestroyPlayersForConnection (conn);


	}



	public override void OnStopHost ()
	{
		Debug.Log ("the server Stopped !!!");

		GameManager.gm.OnServerStop ();

		base.OnStopServer ();

	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		Debug.Log ("Un client a parti");
		//CanvasManager.cm.genericCamera.SetActive (true);

		base.OnClientDisconnect (conn);
	}
}
