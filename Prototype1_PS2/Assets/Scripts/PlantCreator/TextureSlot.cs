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

		this.textureName.text = textureName;
		plantCreatorMaterialManager = PCMM;
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
