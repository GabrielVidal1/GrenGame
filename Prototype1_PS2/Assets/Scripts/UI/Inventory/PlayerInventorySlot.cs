using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerInventorySlot : MonoBehaviour {

	[SerializeField] private TMP_Text plantName;
	[SerializeField] private TMP_Text plantNumber;
	[SerializeField] private RawImage plantTexture;

	PlayerInventoryGrid playerInventoryGrid;

	int inInventoryIndex;

	public void SetPlant(int plantIndex, int number, int inInventoryIndex)
	{
		this.inInventoryIndex = inInventoryIndex;
		plantName.text = GameManager.gm.pm.plantsPrefabs [plantIndex].name;

		plantNumber.text = number > 1 ? number.ToString () : "";

		plantTexture.texture = GameManager.gm.pm.seedsPrefab [plantIndex].seedTexture;
	}

	public void SetInventoryGrid(PlayerInventoryGrid playerInventoryGrid)
	{
		this.playerInventoryGrid = playerInventoryGrid;
	}

	public void OnClick()
	{
		playerInventoryGrid.SetIndex (inInventoryIndex);
	}

			
}
