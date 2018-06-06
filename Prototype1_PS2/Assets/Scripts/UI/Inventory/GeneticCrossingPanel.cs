using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticCrossingPanel : MonoBehaviour {

	private Animator animator;

	public GameObject firstSlot;
	public GameObject secondSlot;

	public float crossingDuration;
	public ClickAndHoldButton button;

	private int firstPlantIndex;
	private int secondPlantIndex;

	private int inInventoryIndex1, inInventoryIndex2;

	private RectTransform firstSlotTransform;
	private RectTransform secondSlotTransform;

	void Start () {
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

		Debug.Log ("IN SLOT 1 = " + inSlot1);
		if (inSlot1) {
			firstPlantIndex = plantIndex;
			inInventoryIndex1 = inInventoryIndex;
			if (firstSlot.transform.childCount > 0)
				Destroy (firstSlot.transform.GetChild (0).gameObject);

			texturePreview.transform.parent = firstSlotTransform.transform;
			texturePreview.transform.localPosition = Vector3.zero;

		} else if (inSlot2) {

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
		Debug.Log ("Click");


		if (firstPlantIndex >= 0 && secondPlantIndex >= 0) {

			Debug.Log ("GOOOOOO CROSSING");

			animator.SetBool ("HoldClick", true);

		} else {


		}
	}

	public void StopProgress()
	{
		animator.SetBool ("HoldClick", false);



	}


	public void ActualCross()
	{
		if (secondSlot.transform.childCount > 0)
			Destroy (secondSlot.transform.GetChild (0).gameObject);
		if (firstSlot.transform.childCount > 0)
			Destroy (firstSlot.transform.GetChild (0).gameObject);
		animator.SetBool ("HoldClick", false);

		inInventoryIndex1 = -1;
		inInventoryIndex2 = -1;




	}
}
