using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryGrid : MonoBehaviour {

	[SerializeField] private GameObject inventoryParent;

	[SerializeField] private GameObject inventoryContent;

	[SerializeField] private PlayerInventorySlot playerInventorySlotPrefab;

	private bool opened;
	public bool Opened
	{ get { return opened; } }

	private bool ready = false;
	public bool IsReady {
		get { return ready; }
	}

	private PlayerInventory playerInventory;

	public void SetPlayer(PlayerInventory playerInventory)
	{
		ready = true;
		this.playerInventory = playerInventory;
	}

	public void OpenInventory()
	{
		if (!ready)
			return;

		inventoryParent.SetActive (true);


		opened = true;

		for (int i = 0; i < playerInventory.inventory.Count; i++) {


			PlayerInventorySlot p = (PlayerInventorySlot)Instantiate (playerInventorySlotPrefab, inventoryContent.transform);

			int index = playerInventory.inventory [i].plantSeedIndexInPlantManager;
			int number = playerInventory.inventory [i].number;

			p.SetInventoryGrid (this);
			p.SetPlant (index, number, i);
		}

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

	}

	public void SetIndex(int index)
	{
		playerInventory.SetIndex (index);
	}


	public void CloseInventory()
	{
		if (!ready)
			return; 

		opened = false;

		for (int i = 0; i < inventoryContent.transform.childCount; i++) {
			Destroy (inventoryContent.transform.GetChild (i).gameObject);
		}

		inventoryParent.SetActive (false);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;


	}

}
