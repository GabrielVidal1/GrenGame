using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeed : Pickup {

	[HideInInspector]
	public int indexInPlantManager;

	public bool inPlantCreator;

	protected override void Init ()
	{
		base.Init ();

		if (!inPlantCreator) {
			Debug.Log ("je suis une graine !    " + name);
			GameManager.gm.pm.plantSeeds.Add (this);
		}
	}

	void Start()
	{
		Init ();
	}

	public override void PickupItem (PlayerInventory playerInventory)
	{
		base.PickupItem (playerInventory);

		playerInventory.AddSeedToInventory (this);
	}

	public string GetPlantName()
	{
		if (GameManager.gm.pm.isPlantDiscoveredFromIndex (indexInPlantManager))
			return GameManager.gm.pm.GetPlantNameFromIndex (indexInPlantManager);
		else
			return "???";
	}
}
