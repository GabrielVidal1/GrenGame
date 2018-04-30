using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSelectionWheel : MonoBehaviour {


	[SerializeField] private SeedSelectionSlot seedRight, seedCenter;


	private int actualIndex;

	PlayerInventory player;

	Animator animator;


	private int leftIndex, RigthIndex;


	bool hasPlayer;

	//[HideInInspector]
	public bool canClick;

	/*
	public void ReactivateFromInactiveState()
	{
		if (player.NbOfSeeds > 0) {
			animator.SetTrigger ("ReactivateFromInactiveState");
			animator.SetBool ("Active", true);
		}
	}
*/

	public void SetPlayer(PlayerInventory playerInventory)
	{
		player = playerInventory;

		CanvasManager.cm.playerInventoryGrid.SetPlayer (playerInventory);

		if (player.NbOfSeeds > 0) {
			seedCenter.SetPlantSeed (player.inventory [actualIndex]);

			animator.SetTrigger ("ReactivateFromInactiveState");
			animator.SetBool ("Active", true);
		}


		hasPlayer = true;
	}

	void Start()
	{
		animator = GetComponent<Animator> ();
		canClick = true;

	}

	public void AddSeed(int index)
	{
		if (player.NbOfSeeds == 1) {
			seedCenter.SetPlantSeed (player.inventory [0]);
			animator.SetBool ("Active", true);
		} else {
			if (index != -1) {
				if (index == actualIndex) {
					//Debug.Log ("actualIndex : " + actualIndex);
					seedCenter.SetPlantSeed (player.inventory [actualIndex]);
				}
			}
		}
	}

	public void UseSeed(int nIndex, bool finishedStack)
	{
		if (nIndex < 0) {
			animator.SetBool ("Active", false);
			actualIndex = 0;
		} else if (finishedStack) {
			seedRight.SetPlantSeed (player.inventory [nIndex]);
			animator.SetTrigger ("UseSeed");
			canClick = false;
		} else {
			seedCenter.SetPlantSeed (player.inventory [actualIndex]);
		}
		actualIndex = nIndex;
	}


	public void SetNewSelectedPlantIndex(int index)
	{
		if (canClick) {
			canClick = false;

			//Debug.Log (actualIndex + " -> " + index + "  / player.inventory.Count = " + player.inventory.Count);

			if (index != actualIndex) {
				if (index - actualIndex == 1 || 
					index - actualIndex == - player.NbOfSeeds + 1 || 
					player.NbOfSeeds == 2) {
					//RIGTH
					seedCenter.SetPlantSeed (player.inventory [index]);
					seedRight.SetPlantSeed (player.inventory [actualIndex]);

					animator.SetTrigger ("RotateRight");

					//print ("RotateRight");

				} else {
					//LEFT	

					seedRight.SetPlantSeed (player.inventory [index]);
					seedCenter.SetPlantSeed (player.inventory [actualIndex]);

					animator.SetTrigger ("RotateLeft");
					//print ("RotateLeft");

				}
				actualIndex = index;

			}
		}
	}

	public void SetIndex(int index)
	{
		actualIndex = index;
		EndRotation ();
	}


	public void EndRotation()
	{
		seedCenter.SetPlantSeed (player.inventory [actualIndex]);

		canClick = true;
	}


}
