using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gm;



	public Player localPlayer;

	public NetworkManager nm;

	public bool isHost;

	public string ipAddressToJoin;

	void Awake() {

		if (gm == null)
			gm = this;
		else if (gm != this)
			Destroy (gameObject);

		nm = GetComponent<NetworkManager> ();
	}


	public void Launch(bool isHost)
	{
		this.isHost = isHost;


		if (isHost) {
			CanvasManager.cm.genericCamera.SetActive (false);
			Debug.Log ("Start Host");

			nm.StartHost ();
		} else {
			Debug.Log ("Start Client");

			nm.networkAddress = ipAddressToJoin;
			Debug.Log (ipAddressToJoin);
			nm.networkPort = 7777;
			nm.StartClient ();
		}
	}

	public void StopGame()
	{
		CanvasManager.cm.genericCamera.SetActive (true);

		localPlayer.DisablePlayer ();

		if (isHost) {
			Debug.Log ("Stop Host");
			nm.StopHost ();
		} else {
			Debug.Log ("Stop Client");
			StopClient ();
		}
	}

	public void StopClient()
	{
		nm.StopClient ();
	}

	public bool IsClientConnected()
	{
		return nm.IsClientConnected ();
	}
}
