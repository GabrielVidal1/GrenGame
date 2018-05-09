using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventoryGrid : MonoBehaviour {

	[SerializeField] private CanvasGroup inventoryParent;

	[SerializeField] private GameObject inventoryContent;

	[SerializeField] private PlayerInventorySlot playerInventorySlotPrefab;



	[SerializeField] private Animator inventoryAnimator;



	[Header("Info Panel")]

	[SerializeField] private TMP_Text plantName;
	[SerializeField] private TMP_Text plantLatinName;
	[SerializeField] private TMP_Text plantDescription;





	private bool isInfoPanelOpened;


	private PlayerInventorySlot selectedSlot;

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

		inventoryParent.alpha = 1f;
		inventoryParent.interactable = true;

		opened = true;

		//Debug.Log (playerInventory.SelectedIndexInInventory);

		for (int i = 0; i < playerInventory.inventory.Count; i++) {


			PlayerInventorySlot p = (PlayerInventorySlot)Instantiate (playerInventorySlotPrefab, inventoryContent.transform);

			int index = playerInventory.inventory [i].plantSeedIndexInPlantManager;
			int number = playerInventory.inventory [i].number;

			p.SetInventoryGrid (this);
			p.SetPlant (index, number, i);

			if (i == playerInventory.SelectedIndexInInventory) {
				selectedSlot = p;
				p.SetSelected ();
				GetInfos (selectedSlot.PlantIndex);
			}

		}

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

	}

	public void SetSelectedSlot(PlayerInventorySlot slot)
	{
		if (selectedSlot != null)
			selectedSlot.Unselect ();

		selectedSlot = slot;
		selectedSlot.SetSelected ();

		GetInfos (selectedSlot.InInventoryIndex);

		playerInventory.SetIndex (selectedSlot.InInventoryIndex);
	}

	public void GetInfos(int index)
	{
		plantName.text = GameManager.gm.pm.plantInformations [index].plantName;
		plantLatinName.text = GameManager.gm.pm.plantInformations [index].plantLatinName;
		plantDescription.text = GameManager.gm.pm.plantInformations [index].plantDescription;
	}


	public void CloseInventory()
	{
		if (!ready)
			return; 

		opened = false;

		for (int i = 0; i < inventoryContent.transform.childCount; i++) {
			Destroy (inventoryContent.transform.GetChild (i).gameObject);
		}

		inventoryParent.alpha = 0f;
		inventoryParent.interactable = false;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;


	}

	public void ToggleInfoPanel()
	{
		if (isInfoPanelOpened)
			CloseInfoPanel ();
		else
			OpenInfoPanel ();
	}


	public void OpenInfoPanel()
	{
		inventoryAnimator.SetBool ("InfoPanelOpened", true);
		isInfoPanelOpened = true;
	}

	public void CloseInfoPanel()
	{
		inventoryAnimator.SetBool ("InfoPanelOpened", false);
		isInfoPanelOpened = false;
	}

}
