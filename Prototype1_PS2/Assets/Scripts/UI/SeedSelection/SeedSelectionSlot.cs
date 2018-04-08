using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SeedSelectionSlot : MonoBehaviour {

	[SerializeField]
	private RawImage seedTexture;

	[SerializeField]
	private TMP_Text plantName;

	[SerializeField]
	private TMP_Text plantNumber;

	public void SetPlantSeed(PlantSeedInventory plantSeed)
	{
		seedTexture.texture = GameManager.gm.pm.seedsPrefab[plantSeed.plantSeedIndexInPlantManager].seedTexture;

		string pName = GameManager.gm.pm.seedsPrefab[plantSeed.plantSeedIndexInPlantManager].GetPlantName ();

		plantName.text = pName;

		plantNumber.text = plantSeed.number < 2 ? "" : plantSeed.number.ToString ();

	}
}
