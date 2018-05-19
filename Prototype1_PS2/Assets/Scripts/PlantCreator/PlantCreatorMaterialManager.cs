using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCreatorMaterialManager : MonoBehaviour {

	[SerializeField] private PlantPartPanelManager plantPartPanelManager;

	[SerializeField] private Material defaultMaterial;

	[SerializeField] private TextureSlot textureSlotPrefab;

	public PlantPartMat[] plantMaterials;
	public PlantPartMat[] leafMaterials;
	public PlantPartMat[] flowerMaterials;
	public PlantPartMat[] fruitMaterials;

	[SerializeField] private Transform trunkParameterList;
	[SerializeField] private Transform branchParameterList;
	[SerializeField] private Transform subBranchParameterList;
	[SerializeField] private Transform leafParameterList;
	[SerializeField] private Transform flowerParameterList;
	[SerializeField] private Transform fruitParameterList;


	private Material plantMat;
	private Material branchMat;
	private Material subBranchMat;
	private Material leafMat;
	private Material flowerMat;
	private Material fruitMat;



	public void Initialize()
	{
		InitializeMaterials ();
	}

	void InitializeMaterials()
	{
		plantMat = new Material (defaultMaterial);
		branchMat = new Material (defaultMaterial);
		subBranchMat = new Material (defaultMaterial);
		leafMat = new Material (defaultMaterial);
		flowerMat = new Material (defaultMaterial);
		fruitMat = new Material (defaultMaterial);

		plantPartPanelManager.targetedPlant.GetComponent<MeshRenderer> ().material = plantMat;
		plantPartPanelManager.targetedBranch.GetComponent<MeshRenderer> ().material = branchMat;
		plantPartPanelManager.targetedSubBranch.GetComponent<MeshRenderer> ().material = subBranchMat;
		plantPartPanelManager.targetedLeaf.GetComponent<MeshRenderer> ().material = leafMat;
		plantPartPanelManager.targetedFlower.GetComponent<MeshRenderer> ().material = flowerMat;
		plantPartPanelManager.targetedFruit.GetComponent<MeshRenderer> ().material = fruitMat;
	}


	public void InitializeMaterialLists()
	{
		for (int i = 0; i < plantMaterials.Length; i++) {

			PlantPartMat p = plantMaterials [i];
			TextureSlot ts = (TextureSlot)Instantiate (textureSlotPrefab, trunkParameterList);
			ts.Initialize (p.textureName, p.albedo, PlantPart.Trunk, this, i);
		}







	}


	public void ChangeTexture(int index, PlantPart pp)
	{
		switch (pp) {

		case PlantPart.Trunk:
			plantMat.mainTexture = plantMaterials [index].albedo;
			plantMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
			break;

		case PlantPart.Branch:
			branchMat.mainTexture = plantMaterials [index].albedo;
			branchMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
			break;

		case PlantPart.SubBranch:
			subBranchMat.mainTexture = plantMaterials [index].albedo;
			subBranchMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
			break;

		case PlantPart.Leaf:
			leafMat.mainTexture = leafMaterials [index].albedo;
			leafMat.SetTexture ("_BumpMap", leafMaterials [index].albedo);
			break;

		case PlantPart.Flower:
			flowerMat.mainTexture = flowerMaterials [index].albedo;
			flowerMat.SetTexture ("_BumpMap", flowerMaterials [index].albedo);
			break;

		case PlantPart.Fruit:
			fruitMat.mainTexture = fruitMaterials [index].albedo;
			fruitMat.SetTexture ("_BumpMap", fruitMaterials [index].normal);
			break;
		}
	}
}

[System.Serializable]
public struct PlantPartMat
{
	public string textureName;
	public Texture albedo;
	public Texture normal;

	public PlantPartMat(string textureName, Texture albedo, Texture normal)
	{
		this.textureName = textureName;
		this.normal = normal;
		this.albedo = albedo;
	}
}