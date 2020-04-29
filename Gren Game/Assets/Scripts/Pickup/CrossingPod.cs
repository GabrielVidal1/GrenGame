using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingPod : Pickup {

	void Start () {
		Init ();
	}

	protected override void Init ()
	{
		base.Init ();
	}

	public override void PickupItem (PlayerInventory playerInventory)
	{
		Debug.Log (name);

		if (playerInventory.nbOfCrossingPods < playerInventory.maxCrossingPods) {

			base.PickupItem (playerInventory);

			playerInventory.GetCrossingPod();

		} else {
			//PLAY "REFUSE SOUND"
		}
	}
}
