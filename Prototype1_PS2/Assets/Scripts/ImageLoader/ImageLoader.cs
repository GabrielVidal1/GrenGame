using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoader : MonoBehaviour {

	[SerializeField] private PlantCreatorMaterialManager plantCreatorMaterialManager;

	void Start () 
	{
		StartCoroutine (LoadTextures());
	}

	public IEnumerator LoadTextures()
	{

		string mainPath = Application.persistentDataPath + "/Textures/";

		string[] folderNames = new [] { "Flowers", "Plants", "Leaves", "Fruits" };

		for (int i = 0; i < folderNames.Length; i++) {

			string path = mainPath + folderNames [i] + "/";

			string[] paths = System.IO.Directory.GetFiles (path, path + folderNames [i] + "Texture_albedo_*.png");

			switch (folderNames [i]) {
			case "Flowers":
				plantCreatorMaterialManager.flowerMaterials = new PlantPartMat[paths.Length];
				break;

			case "Plants":
				plantCreatorMaterialManager.plantMaterials = new PlantPartMat[paths.Length];
				break;

			case "Fruits":
				plantCreatorMaterialManager.fruitMaterials = new PlantPartMat[paths.Length];
				break;

			case "Leaves":
				plantCreatorMaterialManager.leafMaterials = new PlantPartMat[paths.Length];
				break;
			}

			for (int j = 0; j < paths.Length; j++) 
			{

				string textureName = paths [i].Substring (paths [i].LastIndexOf ("_") + 1, paths [i].Length - paths [i].LastIndexOf ("_") - 5);
				Debug.Log (textureName);

				string normalPath = path + folderNames [i] + "Texture_normal_" + textureName + ".png";

				WWW normalW = null;
				bool hasNormalMap = false;

				if (System.IO.File.Exists (normalPath)) {
					normalW = new WWW (normalPath);
					hasNormalMap = true;
				}

				WWW w = new WWW (paths[i]);

				yield return w;

				Texture2D texture = new Texture2D (512, 512);
				Texture2D normal = null;

				if (hasNormalMap)
					normalW.LoadImageIntoTexture (normal);

				w.LoadImageIntoTexture (texture);
				switch (folderNames [i]) {
				case "Flowers":
					plantCreatorMaterialManager.flowerMaterials [j] = new PlantPartMat (textureName, texture, normal);
					break;

				case "Plants":
					plantCreatorMaterialManager.plantMaterials [j] = new PlantPartMat (textureName, texture, normal);
					break;

				case "Fruits":
					plantCreatorMaterialManager.fruitMaterials [j] = new PlantPartMat (textureName, texture, normal);
					 
					break;

				case "Leaves":
					plantCreatorMaterialManager.leafMaterials [j] = new PlantPartMat (textureName, texture, normal);
					break;
				}

				Debug.Log ("Loaded Texture " + paths [i]);
			}
		}

		Debug.Log ("Done Loading Textures");
	}


}
