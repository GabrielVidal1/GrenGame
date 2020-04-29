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


	public Material plantMat;
	public Material branchMat;
	public Material subBranchMat;
	public Material leafMat;
	public Material flowerMat;
	public Material fruitMat;

	public string trunkMatName = "";
	public string branchMatName = "";
	public string subBranchMatName = "";
	public string leafMatName = "";
	public string flowerMatName = "";
	public string fruitMatName = "";

	[SerializeField] private GameObject plant, leaf, flower, fruit;
	private GameObject lastSelected;

	[SerializeField]
	public Dictionary<string, int>[] materialnNamesToIndexes;


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
		CreateLookupTable ();
	}

	void InitializeMaterials()
	{
		plantMat = new Material (defaultMaterial);
		branchMat = new Material (defaultMaterial);
		subBranchMat = new Material (defaultMaterial);
		leafMat = new Material (defaultMaterial);
		flowerMat = new Material (defaultMaterial);
		fruitMat = new Material (defaultMaterial);
		LinkMats ();
	}

	public void LinkMats()
	{
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

	public void CreateLookupTable()
	{
		Debug.Log ("Creating Lookup table");

		materialnNamesToIndexes = new Dictionary<string, int>[4];

		PlantPartMat[][] k = { plantMaterials, leafMaterials, flowerMaterials, fruitMaterials };

		for (int i = 0; i < materialnNamesToIndexes.Length; i++) {

			materialnNamesToIndexes [i] = new Dictionary<string, int> ();

			for (int j = 0; j < k[i].Length; j++) 
			{
				materialnNamesToIndexes [i] [k [i] [j].textureName] = j;
			}
		}
	}


	public void ChangeTexture(string name, PlantPart pp, bool forceInit = false)
	{
		if (forceInit) {
			plantPartSelector.ForceSelect (pp);
			pp = PlantPart.Trunk;
		}

		int index = -1;

		Debug.Log ("TextureName = " + name);

		switch (pp) {

		case PlantPart.Trunk:

			switch (plantPartSelector.SelectedPlantPart) {



			case PlantPart.Trunk:
				if (name == null || name == "") {
					plantMat.mainTexture = defaultMaterial.mainTexture;
					plantMat.SetTexture ("_BumpMap", defaultMaterial.GetTexture("_BumpMap"));
					return;
				}
				index = materialnNamesToIndexes [0] [name];
				plantMat.mainTexture = plantMaterials [index].albedo;
				plantMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
				trunkMatName = name;
				break;
			
			case PlantPart.Branch:
				if (name == null || name == "") {
					branchMat.mainTexture = defaultMaterial.mainTexture;
					branchMat.SetTexture ("_BumpMap", defaultMaterial.GetTexture("_BumpMap"));
					return;
				}
				index = materialnNamesToIndexes [0] [name];

				branchMat.mainTexture = plantMaterials [index].albedo;
				branchMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
				branchMatName = name;
				break;

			case PlantPart.SubBranch:
				if (name == null || name == "") {
					subBranchMat.mainTexture = defaultMaterial.mainTexture;
					subBranchMat.SetTexture ("_BumpMap", defaultMaterial.GetTexture("_BumpMap"));
					return;
				}
				index = materialnNamesToIndexes [0] [name];

				subBranchMat.mainTexture = plantMaterials [index].albedo;
				subBranchMat.SetTexture ("_BumpMap", plantMaterials [index].albedo);
				subBranchMatName = name;
				break;
			}
			break;

		case PlantPart.Leaf:
			if (name == null || name == "") {
				leafMat.mainTexture = defaultMaterial.mainTexture;
				leafMat.SetTexture ("_BumpMap", defaultMaterial.GetTexture("_BumpMap"));
				return;
			}
			index = materialnNamesToIndexes [1] [name];
			leafMat.mainTexture = leafMaterials [index].albedo;
			leafMat.SetTexture ("_BumpMap", leafMaterials [index].albedo);
			leafMatName = name;
			break;

		case PlantPart.Flower:
			if (name == null || name == "") {
				flowerMat.mainTexture = defaultMaterial.mainTexture;
				flowerMat.SetTexture ("_BumpMap", defaultMaterial.GetTexture("_BumpMap"));
				return;
			}
			index = materialnNamesToIndexes [2][name];
			flowerMat.mainTexture = flowerMaterials [index].albedo;
			flowerMat.SetTexture ("_BumpMap", flowerMaterials [index].albedo);
			flowerMatName = name;
			break;

		case PlantPart.Fruit:
			if (name == null || name == "") {
				fruitMat.mainTexture = defaultMaterial.mainTexture;
				fruitMat.SetTexture ("_BumpMap", defaultMaterial.GetTexture("_BumpMap"));
				return;
			}
			index = materialnNamesToIndexes [3] [name];
			fruitMat.mainTexture = fruitMaterials [index].albedo;
			fruitMat.SetTexture ("_BumpMap", fruitMaterials [index].normal);
			fruitMatName = name;
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