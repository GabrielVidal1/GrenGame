using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class PlantSerializer : MonoBehaviour {

	[SerializeField] private PlantData test;

	public void SerializePlant( PlantPartPanelManager PPPM, string name)
	{
		PlantData pd = new PlantData (PPPM.targetedPlant, PPPM.targetedBranch, PPPM.targetedSubBranch, PPPM.targetedLeaf, PPPM.targetedFlower, PPPM.targetedFruit);

		pd.trunkMatIndex = PPPM.plantCreatorMaterialManager.trunkMatIndex;
		pd.branchMatIndex = PPPM.plantCreatorMaterialManager.branchMatIndex;
		pd.subBranchMatIndex = PPPM.plantCreatorMaterialManager.subBranchMatIndex;
		pd.leafMatIndex = PPPM.plantCreatorMaterialManager.leafMatIndex;
		pd.flowerMatIndex = PPPM.plantCreatorMaterialManager.flowerMatIndex;
		pd.fruitMatIndex = PPPM.plantCreatorMaterialManager.fruitMatIndex;

		string s = JsonUtility.ToJson (pd);

		//string s = "";
		//Test (pd, typeof(PlantData), ref s);
		
		string path = Application.persistentDataPath + "/Plants/" + name + ".grenplant";

		System.IO.File.WriteAllText (path, s);
		
	}

	public void Test(object obj, Type type, ref string s)
	{
		System.Reflection.PropertyInfo[] properties = type.GetProperties();

		s += "{";
		
		for (int i = 0; i < properties.Length; i++) {
			
			System.Reflection.PropertyInfo property = properties [i];
			
			Debug.Log("proerty Name : " + property.Name);

			s += "\"" + property.Name + "\":";

			s += JsonUtility.ToJson (property.GetValue (obj, null));

			if (i < properties.Length - 1)
				s += ",";
		}

		s += "}";
		
		Debug.Log (s);
	}


	public Plant UnserializePlant (string path, PlantPartPanelManager PPPM)
	{
		if (File.Exists (path)) {

			string file = System.IO.File.ReadAllText (path);

			test = JsonUtility.FromJson<PlantData> (file);

			JsonUtility.FromJsonOverwrite(test.trunk, PPPM.targetedPlant);
			JsonUtility.FromJsonOverwrite(test.branch, PPPM.targetedBranch);
			JsonUtility.FromJsonOverwrite(test.subBranch, PPPM.targetedSubBranch);

			JsonUtility.FromJsonOverwrite(test.leaf, PPPM.targetedLeaf);
			JsonUtility.FromJsonOverwrite(test.flower, PPPM.targetedFlower);
			JsonUtility.FromJsonOverwrite(test.fruit, PPPM.targetedFruit);

			PPPM.plantCreatorMaterialManager.ChangeTexture (test.trunkMatIndex, PlantPart.Trunk);

			PPPM.plantCreatorMaterialManager.ChangeTexture (test.branchMatIndex, PlantPart.Branch, true);
			PPPM.plantCreatorMaterialManager.ChangeTexture (test.subBranchMatIndex, PlantPart.SubBranch, true);
			PPPM.plantCreatorMaterialManager.ChangeTexture (test.leafMatIndex, PlantPart.Leaf);
			PPPM.plantCreatorMaterialManager.ChangeTexture (test.flowerMatIndex, PlantPart.Flower);
			PPPM.plantCreatorMaterialManager.ChangeTexture (test.fruitMatIndex, PlantPart.Fruit);

			PPPM.RemakeLinks ();
			PPPM.targetedPlant.InitializePlant ();

			Debug.Log ("LOADED PLANT");

		} else {
			throw new FileNotFoundException ();
		}
		return null;
	}
}

[System.Serializable]
public class PlantData
{
	/*
	public string Trunk
	{
		get { return trunk; }
		set { trunk = value; }
	}

	public string Branch
	{
		get { return branch; }
		set { branch = value; }
	}

	public string SubBranch
	{
		get { return subBranch; }
		set { subBranch = value; }
	}

	public string Leaf
	{
		get { return leaf; }
		set { leaf = value; }
	}

	public string Flower
	{
		get { return flower; }
		set { flower = value; }
	}

	public string Fruit
	{
		get { return fruit; }
		set { fruit = value; }
	}
	*/

	public string trunk;
	public string branch;
	public string subBranch;
	public string leaf;
	public string flower;
	public string fruit;

	public int trunkMatIndex;
	public int branchMatIndex;
	public int subBranchMatIndex;
	public int leafMatIndex;
	public int flowerMatIndex;
	public int fruitMatIndex;

	public PlantData(Plant trunk, Plant branch, Plant subBranch, Leaf leaf, Flower flower, Fruit fruit)
	{
		this.trunk = JsonUtility.ToJson (trunk);
		this.branch = JsonUtility.ToJson (branch);
		this.subBranch = JsonUtility.ToJson (subBranch);
		this.leaf = JsonUtility.ToJson (leaf);
		this.flower = JsonUtility.ToJson (flower);
		this.fruit = JsonUtility.ToJson (fruit);
	}
}