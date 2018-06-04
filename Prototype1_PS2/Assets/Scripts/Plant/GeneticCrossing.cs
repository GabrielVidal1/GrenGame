using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticCrossing : MonoBehaviour {

	public GeneticParameterGroup[] trunkGenesGroups;

	public Plant defaultPlant;


	public GameObject GameObjectWithRenderer(string name)
	{
		return new GameObject (name, typeof(MeshFilter), typeof(MeshRenderer));
	}


	public Plant CrossPlants(Plant p1, Plant p2, int randomSeed)
	{
		GameObject parent = new GameObject ("PlantParent");
		Plant trunk = (Plant)Instantiate (defaultPlant, parent.transform);

		foreach (GeneticParameterGroup group in trunkGenesGroups) {

			foreach (string propertyName in group.parameters) {
				
				object value = null;

				if (RandomBool ())
					value = typeof(Plant).GetProperty (propertyName).GetValue (p1, null);
				else
					value = typeof(Plant).GetProperty (propertyName).GetValue (p2, null);
				
				typeof(Plant).GetProperty (propertyName).SetValue (trunk, value, null);
			}
		}

		return trunk;
	}

	public bool RandomBool()
	{
		return Random.value < 0.5f;
	}

}

public struct GeneticParameterGroup
{
	public string[] parameters;
}
