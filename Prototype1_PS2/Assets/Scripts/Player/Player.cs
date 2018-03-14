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
		GameManager.gm.localPlayer = this;

		onToggleShared.Invoke (true);

		if (isLocalPlayer)
			onToggleLocal.Invoke (true);
		else
			onToggleRemote.Invoke (true);
	}

}
