using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoader : MonoBehaviour {

	[SerializeField] private PlantCreatorMaterialManager plantCreatorMaterialManager;

	[SerializeField] private Vector2 plantsTextureSize;
	[SerializeField] private Vector2 leavesTextureSize;
	[SerializeField] private Vector2 flowersTextureSize;
	[SerializeField] private Vector2 fruitsTextureSize;



	public void LoadTextures()
	{
		StartCoroutine (_LoadTextures ());
	}


	IEnumerator _LoadTextures()
	{
		Vector2[] textureSizes = new [] { plantsTextureSize, leavesTextureSize, flowersTextureSize, fruitsTextureSize };

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

				//Debug.Log ("Loading '" + paths [j] + "'");
				string textureName = paths [j].Substring (paths [j].LastIndexOf ("_") + 1, paths [j].Length - paths [j].LastIndexOf ("_") - 5);
				//Debug.Log (textureName);

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

				//Debug.Log ("Loaded Texture " + paths [j]);
			}
		}

		Debug.Log ("Done Loading Textures");
		plantCreatorMaterialManager.Initialize ();
	}


}
