using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class PlayerInventorySlot : MonoBehaviour,  IDragHandler, IPointerUpHandler, IPointerDownHandler{

	[SerializeField] private TMP_Text plantName;
	[SerializeField] private TMP_Text plantNumber;
	[SerializeField] private RawImage plantTexture;

	[SerializeField] private RawImage selectionMark;

	PlayerInventoryGrid playerInventoryGrid;
	private GeneticCrossingPanel geneticCrossingPanel;

	int inInventoryIndex;

	public int InInventoryIndex {
		get { return inInventoryIndex; }
	}

	int plantIndex;

	public int PlantIndex
	{ get { return plantIndex; } }

	public void SetPlant(int plantIndex, int number, int inInventoryIndex)
	{
		this.inInventoryIndex = inInventoryIndex;
		this.plantIndex = plantIndex;

		plantName.text = GameManager.gm.pm.plantInformations [plantIndex].plantName;

		plantNumber.text = number > 1 ? number.ToString () : "";

		plantTexture.texture = GameManager.gm.pm.plantInformations [plantIndex].plantTexture;
	}

	public void SetInventoryGrid(PlayerInventoryGrid playerInventoryGrid)
	{
		this.playerInventoryGrid = playerInventoryGrid;
		geneticCrossingPanel = playerInventoryGrid.geneticCrossingPanel.GetComponent<GeneticCrossingPanel>();
	}

	public void SetSelected()
	{
		selectionMark.gameObject.SetActive (true);
	}

	public void Unselect()
	{
		selectionMark.gameObject.SetActive (false);
	}

	public void OnClick()
	{
		playerInventoryGrid.SetSelectedSlot (this);
	}


	private bool dragging;
	private GameObject texturePreviewDrag;
	private Canvas mainCanvas;

	public void OnPointerDown(PointerEventData data)
	{
		if (playerInventoryGrid.GeneticTabOpened) {
			texturePreviewDrag = (GameObject)Instantiate (plantTexture.gameObject, transform);
			dragging = true;
		} else {
			OnClick ();
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		if (dragging) {
			dragging = false;

			geneticCrossingPanel.DropPlant (texturePreviewDrag, plantIndex, inInventoryIndex);

		} else {
			OnClick ();
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (dragging) {

			texturePreviewDrag.transform.position = Input.mousePosition;

		}
	}

}
