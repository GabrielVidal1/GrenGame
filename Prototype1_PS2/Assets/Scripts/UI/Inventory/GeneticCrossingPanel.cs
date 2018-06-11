using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GeneticCrossingPanel : MonoBehaviour {

	private Animator animator;

	public GameObject firstSlot;
	public GameObject secondSlot;

	public float crossingDuration;
	public ClickAndHoldButton button;

	[SerializeField] private NewPlantPanel newPlantPanel;

	private int firstPlantIndex, secondPlantIndex;

	private int inInventoryIndex1, inInventoryIndex2;

	private RectTransform firstSlotTransform;
	private RectTransform secondSlotTransform;

	private PlayerInventoryGrid playerInventoryGrid;

	public void SetPlayerInventoryGrid(PlayerInventoryGrid pig)
	{
		playerInventoryGrid = pig;

		animator = GetComponent<Animator> ();
		firstSlotTransform = firstSlot.GetComponent<RectTransform> ();
		secondSlotTransform = secondSlot.GetComponent<RectTransform> ();

		firstPlantIndex = -1;
		secondPlantIndex = -1;
		button.holdingDuration = crossingDuration;

		//animator.speed = 1f / crossingDuration;
	}

	public void DropPlant(GameObject texturePreview, int plantIndex, int inInventoryIndex)
	{
		Vector3 pos;

		bool inSlot1 = RectTransformUtility.RectangleContainsScreenPoint (firstSlotTransform, Input.mousePosition, null);
		bool inSlot2 = RectTransformUtility.RectangleContainsScreenPoint (secondSlotTransform, Input.mousePosition, null);

		if (inSlot1) {
			if (secondPlantIndex == plantIndex) {
				Debug.Log (secondPlantIndex + "  " + plantIndex);
				animator.SetTrigger ("Refuse");
				Destroy (texturePreview);
			}


			firstPlantIndex = plantIndex;
			inInventoryIndex1 = inInventoryIndex;
			if (firstSlot.transform.childCount > 0)
				Destroy (firstSlot.transform.GetChild (0).gameObject);

			texturePreview.transform.parent = firstSlotTransform.transform;
			texturePreview.transform.localPosition = Vector3.zero;

		} else if (inSlot2) {

			if (firstPlantIndex == plantIndex) {
				Debug.Log (firstPlantIndex + "  " + plantIndex);

				animator.SetTrigger ("Refuse");
				Destroy (texturePreview);
			}

			secondPlantIndex = plantIndex;
			inInventoryIndex2 = inInventoryIndex;

			if (secondSlot.transform.childCount > 0)
				Destroy (secondSlot.transform.GetChild (0).gameObject);

			texturePreview.transform.parent = secondSlotTransform.transform;
			texturePreview.transform.localPosition = Vector3.zero;

		} else {

			Destroy (texturePreview);

		}
	}

	public void Cross()
	{
		Debug.Log ("Called Cross()");

		if (!playerInventoryGrid.CanCross) {
			animator.SetTrigger ("NotEnough");
			return;
		}

		if (firstPlantIndex >= 0 && secondPlantIndex >= 0) {

			if (!GameManager.gm.pm.plantsPrefabs [firstPlantIndex].isCrossing &&
			    !GameManager.gm.pm.plantsPrefabs [secondPlantIndex].isCrossing) {

				if (GameManager.gm.pm.plantsPrefabs [firstPlantIndex].crossingFamily == 
					GameManager.gm.pm.plantsPrefabs [secondPlantIndex].crossingFamily) {

					Debug.Log ("GOOOOOO CROSSING");
					animator.SetBool ("HoldClick", true);
				}
			}
		} else {
			animator.SetTrigger ("Refuse");


		}
	}

	public void StopProgress()
	{
		animator.SetBool ("HoldClick", false);



	}

	public void CreateCrossedPlant(bool textureFromParent1, string name)
	{
		int index = GameManager.gm.pm.plantsPrefabs.Count;

		PlayerInventory pi = GameManager.gm.localPlayer.GetComponent<PlayerInventory> ();

		pi.CmdAddPlantToPlantManagerFromParents (
			index,
			firstPlantIndex, secondPlantIndex,
			textureFromParent1 ? firstPlantIndex : secondPlantIndex,
			name);

		Debug.Log ("index == " + (textureFromParent1 ? firstPlantIndex : secondPlantIndex));

		firstPlantIndex = -1;
		secondPlantIndex = -1;

		pi.AddSeedToInventoryFromIndex (index);

	}


	public void ActualCross()
	{
		animator.SetBool ("HoldClick", false);

		inInventoryIndex1 = -1;
		inInventoryIndex2 = -1;



		playerInventoryGrid.Cross ();

		newPlantPanel.gameObject.SetActive (true);
		newPlantPanel.plantIcon1.texture = firstSlot.transform.GetChild(0).GetComponent<RawImage> ().texture;
		newPlantPanel.plantIcon2.texture = secondSlot.transform.GetChild(0).GetComponent<RawImage> ().texture;

		newPlantPanel.Init ();

		if (secondSlot.transform.childCount > 0)
			Destroy (secondSlot.transform.GetChild (0).gameObject);
		if (firstSlot.transform.childCount > 0)
			Destroy (firstSlot.transform.GetChild (0).gameObject);
		//CALL CROSSING WITH GENETIC CROSSING SCRIP
		//CREATE NEW PREFAB


	}
}
