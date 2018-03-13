using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> {}

public class Player : NetworkBehaviour {

	[SerializeField] ToggleEvent onToggleLocal;
	[SerializeField] ToggleEvent onToggleShared;
	[SerializeField] ToggleEvent onToggleRemote;


	void Start()
	{
		EnablePlayer ();
	}

	void Update()
	{

	}

	void DisablePlayer()
	{

		GameManager.gm.cManager.genericCamera.SetActive (true);

	}

	void EnablePlayer()
	{
		GameManager.gm.cManager.genericCamera.SetActive (false);


		onToggleShared.Invoke (true);

		if (isLocalPlayer)
			onToggleLocal.Invoke (true);
		else
			onToggleRemote.Invoke (true);
	}



	void OnDisconnectedFromServer(NetworkDisconnection nd)
	{
		if (isLocalPlayer) {
			DisablePlayer ();
		}

	}
}
