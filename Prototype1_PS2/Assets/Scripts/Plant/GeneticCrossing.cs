using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GeneticCrossing : MonoBehaviour {

	public GeneticParameterGroup[] trunkGenesGroups;

	public GameObject GameObjectWithRenderer(string name)
	{
		return new GameObject (name, typeof(MeshFilter), typeof(MeshRenderer));
	}




	public void AddPlantToPlantManagerFromParents(int newPlantIndex, int parentIndex1, int parentIndex2, int plantTextureIndex, string plantName)
	{
		while (GameManager.gm.pm.plantsPrefabs.Count < newPlantIndex) {
			GameManager.gm.pm.plantsPrefabs.Add (null);
		}

		Plant p = CrossPlants (GameManager.gm.pm.plantsPrefabs [parentIndex1],
			          GameManager.gm.pm.plantsPrefabs [parentIndex2], newPlantIndex);

		p.name = plantName;

		p.parentIndex1 = parentIndex1;
		p.parentIndex2 = parentIndex2;

		p.plantTypeIndex = newPlantIndex;

		p.plantTextureIndex = plantTextureIndex;

		GameManager.gm.pm.plantsPrefabs.Add (p);
		GameManager.gm.pm.crossedPlants.Add (p);

		PlantInformation pinfo = new PlantInformation (plantName);
		pinfo.plantTexture = GameManager.gm.pm.plantInformations[plantTextureIndex].plantTexture;

		pinfo.plantLatinName = GameManager.gm.pm.plantInformations [parentIndex1].plantLatinName
			+ " " + GameManager.gm.pm.plantInformations [parentIndex2].plantLatinName;

		pinfo.plantDescription = "This plant is worth " + p.pointValue + " points. " +
		"This plant is coss-breeding between " + GameManager.gm.pm.plantInformations [parentIndex1].plantName
		+ " and " + GameManager.gm.pm.plantInformations [parentIndex2].plantName;
		
		GameManager.gm.pm.plantInformations.Add (pinfo);

	}

	public Plant CrossPlants(Plant p1, Plant p2, int randomSeed)
	{
		//Material plantMat = p2.GetComponent<MeshRenderer> ().sharedMaterial;



		GameObject parent = new GameObject ("PlantParent");
		Plant trunk = (Plant)Instantiate (p1, parent.transform);
		trunk.pointValue += p2.pointValue;

		trunk.isCrossing = true;




		foreach (GeneticParameterGroup group in trunkGenesGroups) {

			foreach (string propertyName in group.parameters) {
				
				//object value = null;

				if (RandomBool ()) {
					typeof(Plant).GetProperty (propertyName).
					SetValue (trunk, typeof(Plant).GetProperty (propertyName).GetValue (p2, null), null);
				
				}
			}
		}

		return trunk;
	}

	public bool RandomBool()
	{
		return Random.value < 0.5f;
	}

}

[System.Serializable]
public struct GeneticParameterGroup
{
	public string[] parameters;
}
