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

	public Dictionary<string, int>[] materialnNamesToIndexes;

	private PlantManager pm;



	void Start()
	{
		Initialize ();

	}




	public void CreateLookupTable()
	{
		Debug.Log ("Creating Lookup table");

		materialnNamesToIndexes = new Dictionary<string, int>[4];

		Material[][] k = { plantMaterials, leafMaterials, flowerMaterials, fruitMaterials };

		for (int i = 0; i < materialnNamesToIndexes.Length; i++) {

			materialnNamesToIndexes [i] = new Dictionary<string, int> ();

			for (int j = 0; j < k[i].Length; j++) 
			{
				materialnNamesToIndexes [i] [k [i] [j].name] = j;
			}
		}
	}


	public GameObject GameObjectWithRenderer(string name)
	{
		return new GameObject (name, typeof(MeshFilter), typeof(MeshRenderer));
	}

	public void Initialize()
	{
		pm = GetComponent<PlantManager> ();
		
		StartCoroutine (InitializeLoading ());
	}

	IEnumerator InitializeLoading()
	{
		yield return StartCoroutine(LoadMaterials ());
		LoadPlants ();
		pm.Init ();
	}


	public void LoadPlants()
	{
		Debug.Log ("Creating Plants");

		string plantFolderPath = Application.persistentDataPath + "/Plants/";

		if (!Directory.Exists (plantFolderPath))
			Directory.CreateDirectory (plantFolderPath);

		string[] plantPaths = System.IO.Directory.GetFiles (plantFolderPath);

		for (int i = 0; i < plantPaths.Length; i++) {

			string path = plantPaths [i];

			if (path.Contains (".grenplant")) {

				string plantName = path.Substring (path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 11);
				//string textureName = paths [j].Substring (paths [j].LastIndexOf ("_") + 1, paths [j].Length - paths [j].LastIndexOf ("_") - 5);

				Debug.Log ("plantName = " + plantName);

				Plant p = CreatePrefabFromFile (path, plantName);

				pm.plantsPrefabs.Add(p);

				PlantInformation pinfo = new PlantInformation (p.name);
				pinfo.plantTexture = p.leafPrefab.GetComponent<MeshRenderer> ().material.mainTexture;

				pm.plantInformations.Add (pinfo);

			}
		}
	}



	public Plant CreatePrefabFromFile(string path, string plantName)
	{
		if (File.Exists (path)) {

			GameObject parent = new GameObject (plantName);
			parent.transform.SetParent (transform);

			Plant trunk = GameObjectWithRenderer (plantName).AddComponent<Plant> ();
			trunk.gameObject.AddComponent<TreeGrowth> ();

			Plant branch = GameObjectWithRenderer ("Branch").AddComponent<Plant> ();
			Plant subBranch = GameObjectWithRenderer ("SubBranch").AddComponent<Plant> ();

			Leaf leaf = GameObjectWithRenderer ("Leaf").AddComponent<Leaf> ();
			Flower flower = GameObjectWithRenderer ("Flower").AddComponent<Flower> ();
			GameObject fruitg = GameObjectWithRenderer ("Fruit").AddComponent<SphereCollider> ().gameObject;
			Fruit fruit = fruitg.AddComponent<Fruit> ();


			trunk.transform.SetParent (parent.transform);
			branch.transform.SetParent (parent.transform);
			subBranch.transform.SetParent (parent.transform);
			leaf.transform.SetParent (parent.transform);
			flower.transform.SetParent (parent.transform);
			fruit.transform.SetParent (parent.transform);


			string file = System.IO.File.ReadAllText (path);

			PlantData test = JsonUtility.FromJson<PlantData> (file);

			JsonUtility.FromJsonOverwrite(test.trunk, trunk);
			JsonUtility.FromJsonOverwrite(test.branch, branch);
			JsonUtility.FromJsonOverwrite(test.subBranch, subBranch);

			JsonUtility.FromJsonOverwrite(test.leaf, leaf);
			JsonUtility.FromJsonOverwrite(test.flower, flower);
			JsonUtility.FromJsonOverwrite(test.fruit, fruit);

			trunk.GetComponent<MeshRenderer> ().material = GetMaterial (test.trunkMatName, PlantPart.Trunk);
			branch.GetComponent<MeshRenderer> ().material = GetMaterial (test.branchMatName, PlantPart.Branch);
			subBranch.GetComponent<MeshRenderer> ().material = GetMaterial (test.subBranchMatName, PlantPart.SubBranch);
			leaf.GetComponent<MeshRenderer> ().material = GetMaterial (test.leafMatName, PlantPart.Leaf);
			flower.GetComponent<MeshRenderer> ().material = GetMaterial (test.flowerMatName, PlantPart.Flower);
			fruit.GetComponent<MeshRenderer> ().material = GetMaterial (test.fruitMatName, PlantPart.Fruit);


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

			return trunk;


		} else {
			throw new FileNotFoundException ();
		}
	}


	void CreateMaterialFromPlantPartMat(PlantPartMat ppm, ref Material mat)
	{
		mat = new Material (defaultMaterial);
		mat.mainTexture = ppm.albedo;
		mat.SetTexture ("_BumpMap", ppm.albedo);
	}

	Material GetMaterial(string textureName, PlantPart plantPart)
	{
		Debug.Log ("Get Mat with '" + textureName + "'");


		if (textureName == null || textureName == "")
			return defaultMaterial;

		if (plantPart == PlantPart.Trunk ||
		    plantPart == PlantPart.Branch ||
		    plantPart == PlantPart.SubBranch) {
			return plantMaterials [materialnNamesToIndexes [0] [textureName]];
		} else if (plantPart == PlantPart.Leaf) {
			return leafMaterials [materialnNamesToIndexes [1] [textureName]];

		} else if (plantPart == PlantPart.Flower) {
			return flowerMaterials [materialnNamesToIndexes [2] [textureName]];
		} else {
			return fruitMaterials [materialnNamesToIndexes [3] [textureName]];
		}
	}



	IEnumerator LoadMaterials()
	{
		Vector2[] textureSizes = new [] { plantsTextureSize, leavesTextureSize, flowersTextureSize, fruitsTextureSize };

		string mainPath = Application.persistentDataPath + "/Textures/";

		string[] folderNames = new [] { "Flowers", "Plants", "Leaves", "Fruits" };

		for (int i = 0; i < folderNames.Length; i++) {

			string path = mainPath + folderNames [i] + "/";

			if (!Directory.Exists (path))
				Directory.CreateDirectory (path);

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
				m.name = textureName;
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
		CreateLookupTable ();
	}

}