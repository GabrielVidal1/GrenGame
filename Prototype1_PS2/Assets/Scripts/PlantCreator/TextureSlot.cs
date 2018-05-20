using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TextureSlot : MonoBehaviour {

	[SerializeField] private RawImage rawImage;
	[SerializeField] private TMP_Text textureName;


	private PlantPart plantPart;
	private PlantCreatorMaterialManager plantCreatorMaterialManager;
	private int index;
	public void Initialize (string textureName, Texture texture, PlantPart plantPart, PlantCreatorMaterialManager PCMM, int index)
	{
		this.index = index;
		this.plantPart = plantPart;
		rawImage.texture = texture;

		this.textureName.text = DisplayParameter.NicifyName(textureName);
		plantCreatorMaterialManager = PCMM;

		if (plantPart == PlantPart.Leaf) {
			rawImage.rectTransform.rotation = Quaternion.Euler (0f, 0f, 90f);
		}

		if (plantPart == PlantPart.Flower) {
			rawImage.rectTransform.sizeDelta = new Vector2 (rawImage.rectTransform.sizeDelta.x, rawImage.rectTransform.sizeDelta.y * 2f);
			rawImage.rectTransform.localPosition = new Vector3 (0f, 107f, 0f);
		}
	}

	public void OnClick()
	{
		plantCreatorMaterialManager.ChangeTexture (index, plantPart);
	}


	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}
}
