using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class CustomNetworkManager : NetworkManager {

	public override void OnClientConnect (NetworkConnection conn)
	{
		Debug.Log ("Un client a rejoin");

		CanvasManager.cm.genericCamera.SetActive (false);
		
		base.OnClientConnect (conn);

	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		Debug.Log ("Un client a parti");
		CanvasManager.cm.genericCamera.SetActive (true);

		base.OnClientDisconnect (conn);
	}
}
