using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCreatorMaterialManager : MonoBehaviour {

	[SerializeField] private PlantPartPanelManager plantPartPanelManager;

	[SerializeField] private Material defaultMaterial;

	[SerializeField] private TextureSlot textureSlotPrefab;

	[SerializeField] private PlantPartSelector plantPartSelector;

	public PlantPartMat[] plantMaterials;
	public PlantPartMat[] leafMaterials;
	public PlantPartMat[] flowerMaterials;
	public PlantPartMat[] fruitMaterials;

	[SerializeField] private Transform plantParameterList;
	[SerializeField] private Transform leafParameterList;
	[SerializeField] private Transform flowerParameterList;
	[SerializeField] private Transform fruitParameterList;


	private Material plantMat;
	private Material branchMat;
	private Material subBranchMat;
	private Material leafMat;
	private Material flowerMat;
	private Material fruitMat;

	public int trunkMatIndex = -1;
	public int branchMatIndex = -1;
	public int subBranchMatIndex = -1;
	public int leafMatIndex = -1;
	public int flowerMatIndex = -1;
	public int fruitMatIndex = -1;

	[SerializeField] private GameObject plant, leaf, flower, fruit;
	private GameObject lastSelected;


	public void TogglePlant()
	{
		lastSelected.SetActive (false);
		plant.SetActive (true);
		lastSelected = plant;
	}

	public void ToggleLeaf()
	{
		lastSelected.SetActive (false);
		leaf.SetActive (true);
		lastSelected = leaf;
	}

	public void ToggleFlower()
	{
		lastSelected.SetActive (false);
		flower.SetActive (true);
		lastSelected = flower;
	}

	public void ToggleFruit()
	{
		lastSelected.SetActive (false);
		fruit.SetActive (true);
		lastSelected = fruit;
	}

	public void Initialize()
	{
		InitializeMaterials ();
		InitializeMaterialLists ();

		lastSelected = plant.gameObject;

		plant.gameObject.SetActive (true);
		leaf.gameObject.SetActive (false);
		flower.gameObject.SetActive (false);
		fruit.gameObject.SetActive (false);

		plantPartSelector.Initialize ();

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
			TextureSlot ts = (TextureSlot)Instantiate (textureSlotPrefab, plantParameterList);
			ts.Initialize (p.textureName, p.albedo, PlantPart.Trunk, this, i);
		}

		for (int i = 0; i < flowerMaterials.Length; i++) {

			PlantPartMat p = flowerMaterials [i];
			TextureSlot ts = (TextureSlot)Instantiate (textureSlotPrefab, flowerParameterList);
			ts.Initialize (p.textureName, p.albedo, PlantPart.Flower, this, i);
		}

		for (int i = 0; i < leafMaterials.Length; i++) {

			PlantPartMat p = leafMaterials [i];
			TextureSlot ts = (TextureSlot)Instantiate (textureSlotPrefab, leafParameterList);
			ts.Initialize (p.textureName, p.albedo, PlantPart.Leaf, this, i);
		}


	}


	public void ChangeTexture(int index, PlantPart pp, bool forceInit = false)
	{
		if (forceInit) {
			plantPartSelector.ForceSelect (pp);
			pp = PlantPart.Trunk;
		}

		//Debug.Log (pp + "   " + plantPartSelector.SelectedPlantPart);

		switch (pp) {

		case PlantPart.Trunk:

			switch (plantPartSelector.SelectedPlantPart) {
			case PlantPart.Trunk:
				if (index == -1) {
					plantMat = defaultMaterial;
					return;
				}
				plantMat.mainTexture = plantMaterials [index].albedo;
				plantMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
				trunkMatIndex = index;
				break;
			
			case PlantPart.Branch:
				if (index == -1) {
					branchMat = defaultMaterial;
					return;
				}
				branchMat.mainTexture = plantMaterials [index].albedo;
				branchMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
				branchMatIndex = index;
				break;

			case PlantPart.SubBranch:
				if (index == -1) {
					subBranchMat = defaultMaterial;
					return;
				}
				subBranchMat.mainTexture = plantMaterials [index].albedo;
				subBranchMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
				subBranchMatIndex = index;
				break;
			}
			break;

		case PlantPart.Leaf:
			if (index == -1) {
				leafMat = defaultMaterial;
				return;
			}
			leafMat.mainTexture = leafMaterials [index].albedo;
			leafMat.SetTexture ("_BumpMap", leafMaterials [index].albedo);
			leafMatIndex = index;
			break;

		case PlantPart.Flower:
			if (index == -1) {
				flowerMat = defaultMaterial;
				return;
			}
			flowerMat.mainTexture = flowerMaterials [index].albedo;
			flowerMat.SetTexture ("_BumpMap", flowerMaterials [index].albedo);
			flowerMatIndex = index;
			break;

		case PlantPart.Fruit:
			if (index == -1) {
				fruitMat = defaultMaterial;
				return;
			}
			fruitMat.mainTexture = fruitMaterials [index].albedo;
			fruitMat.SetTexture ("_BumpMap", fruitMaterials [index].normal);
			fruitMatIndex = index;
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