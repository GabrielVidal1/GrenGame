using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlantPrefabCreator : MonoBehaviour {

	[SerializeField] private Vector2 plantsTextureSize;
	[SerializeField] private Vector2 leavesTextureSize;
	[SerializeField] private Vector2 flowersTextureSize;
	[SerializeField] private Vector2 fruitsTextureSize;

	[SerializeField] private Material defaultMaterial;

	public Material[] plantMaterials;
	public Material[] leafMaterials;
	public Material[] flowerMaterials;
	public Material[] fruitMaterials;

	public GameObject GameObjectWithRenderer(string name)
	{
		return new GameObject (name, typeof(MeshFilter), typeof(MeshRenderer));
	}




	public Plant CreatePrefabFromFile(string path)
	{
		if (File.Exists (path)) {
			Plant trunk = GameObjectWithRenderer ("Trunk").AddComponent<Plant> ();
			Plant branch = GameObjectWithRenderer ("Branch").AddComponent<Plant> ();
			Plant subBranch = GameObjectWithRenderer ("SubBranch").AddComponent<Plant> ();

			Leaf leaf = GameObjectWithRenderer ("Leaf").AddComponent<Leaf> ();
			Flower flower = GameObjectWithRenderer ("Flower").AddComponent<Flower> ();
			Fruit fruit = GameObjectWithRenderer ("Fruit").AddComponent<Fruit> ();
			fruit.gameObject.AddComponent<SphereCollider> ();


			string file = System.IO.File.ReadAllText (path);

			PlantData test = JsonUtility.FromJson<PlantData> (file);

			JsonUtility.FromJsonOverwrite(test.trunk, trunk);
			JsonUtility.FromJsonOverwrite(test.branch, branch);
			JsonUtility.FromJsonOverwrite(test.subBranch, subBranch);

			JsonUtility.FromJsonOverwrite(test.leaf, leaf);
			JsonUtility.FromJsonOverwrite(test.flower, flower);
			JsonUtility.FromJsonOverwrite(test.fruit, fruit);

			trunk.GetComponent<MeshRenderer> ().material = GetMaterial (test.trunkMatIndex, PlantPart.Trunk);
			branch.GetComponent<MeshRenderer> ().material = GetMaterial (test.branchMatIndex, PlantPart.Branch);
			subBranch.GetComponent<MeshRenderer> ().material = GetMaterial (test.subBranchMatIndex, PlantPart.SubBranch);
			leaf.GetComponent<MeshRenderer> ().material = GetMaterial (test.leafMatIndex, PlantPart.Leaf);
			flower.GetComponent<MeshRenderer> ().material = GetMaterial (test.flowerMatIndex, PlantPart.Flower);
			fruit.GetComponent<MeshRenderer> ().material = GetMaterial (test.fruitMatIndex, PlantPart.Fruit);


			//LINKS
			trunk.branchPrefab = branch;
			trunk.leafPrefab = leaf;
			trunk.flowerPrefab = flower;
			trunk.fruitPrefab = fruit;

			branch.branchPrefab = subBranch;
			branch.leafPrefab = leaf;
			branch.flowerPrefab = flower;
			branch.fruitPrefab = fruit;

			subBranch.leafPrefab = leaf;
			subBranch.flowerPrefab = flower;
			subBranch.fruitPrefab = fruit;

		} else {
			throw new FileNotFoundException ();
		}
		return null;
	}


	void CreateMaterialFromPlantPartMat(PlantPartMat ppm, ref Material mat)
	{
		mat = new Material (defaultMaterial);
		mat.mainTexture = ppm.albedo;
		mat.SetTexture ("_BumpMap", ppm.albedo);
	}

	Material GetMaterial(int index, PlantPart plantPart)
	{
		if (index == -1)
			return defaultMaterial;

		if (plantPart == PlantPart.Trunk ||
		    plantPart == PlantPart.Branch ||
		    plantPart == PlantPart.SubBranch) {
			return plantMaterials [index];
		} else if (plantPart == PlantPart.Leaf) {
			return leafMaterials [index];

		} else if (plantPart == PlantPart.Flower) {
			return flowerMaterials [index];
		} else {
			return fruitMaterials [index];
		}
	}



	IEnumerator LoadMaterials()
	{
		Vector2[] textureSizes = new [] { plantsTextureSize, leavesTextureSize, flowersTextureSize, fruitsTextureSize };

		string mainPath = Application.persistentDataPath + "/Textures/";

		string[] folderNames = new [] { "Flowers", "Plants", "Leaves", "Fruits" };

		for (int i = 0; i < folderNames.Length; i++) {

			string path = mainPath + folderNames [i] + "/";

			string[] paths = System.IO.Directory.GetFiles (path, path + folderNames [i] + "Texture_albedo_*.png");

			switch (folderNames [i]) {
			case "Flowers":
				flowerMaterials = new Material[paths.Length];
				break;

			case "Plants":
				plantMaterials = new Material[paths.Length];
				break;

			case "Fruits":
				fruitMaterials = new Material[paths.Length];
				break;

			case "Leaves":
				leafMaterials = new Material[paths.Length];
				break;
			}

			for (int j = 0; j < paths.Length; j++) 
			{

				Debug.Log ("Loading '" + paths [j] + "'");
				string textureName = paths [j].Substring (paths [j].LastIndexOf ("_") + 1, paths [j].Length - paths [j].LastIndexOf ("_") - 5);
				Debug.Log (textureName);

				string normalPath = path + folderNames [i] + "Texture_normal_" + textureName + ".png";

				WWW normalW = null;
				bool hasNormalMap = false;

				if (System.IO.File.Exists (normalPath)) {
					normalW = new WWW (normalPath);
					hasNormalMap = true;
				}

				WWW w = new WWW (paths[j]);

				yield return w;

				Texture2D texture = new Texture2D ((int)textureSizes[i].x, (int)textureSizes[i].y, TextureFormat.ARGB32, true);
				Texture2D normal = null;

				if (hasNormalMap) {
					normal = new Texture2D ((int)textureSizes[i].x, (int)textureSizes[i].y, TextureFormat.ARGB32, true);
					normalW.LoadImageIntoTexture (normal);
				}
				w.LoadImageIntoTexture (texture);

				Material m = new Material (defaultMaterial);
				m.mainTexture = texture;
				m.SetTexture ("_BumMap", normal);

				switch (folderNames [i]) {
				case "Flowers":
					flowerMaterials [j] = m;
					break;

				case "Plants":
					plantMaterials [j] = m;
					break;

				case "Fruits":
					fruitMaterials [j] = m;
					break;

				case "Leaves":
					leafMaterials [j] = m;
					break;
				}

				Debug.Log ("Loaded Texture " + paths [j]);
			}
		}

		Debug.Log ("Done Loading Textures");
	}

}
