using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterListManager : MonoBehaviour {





	private PlantPartPanelManager plantPartPanelManager;

	[SerializeField] private Transform content;
	[SerializeField] private ContentHeightAdapter contentParent;

	public void Initialize(PlantPartPanelManager plantPartPanelManager)
	{
		this.plantPartPanelManager = plantPartPanelManager;
		InitializeParameterList ();
	}


	public void UpdatePlant(string propertyName, object value, PlantPart pp)
	{
		//Debug.Log (propertyName);

		switch (pp) {
		case PlantPart.Trunk:
			typeof(Plant).GetProperty (propertyName).SetValue (plantPartPanelManager.targetedPlant, value, null);
			break;

		case PlantPart.Branch:
			typeof(Plant).GetProperty (propertyName).SetValue (plantPartPanelManager.targetedBranch, value, null);
			break;

		case PlantPart.SubBranch:
			typeof(Plant).GetProperty (propertyName).SetValue (plantPartPanelManager.targetedSubBranch, value, null);
			break;

		case PlantPart.Leaf:
			typeof(Leaf).GetProperty (propertyName).SetValue (plantPartPanelManager.targetedLeaf, value, null);
			break;

		case PlantPart.Flower:
			typeof(Flower).GetProperty (propertyName).SetValue (plantPartPanelManager.targetedFlower, value, null);
			break;

		case PlantPart.Fruit:
			typeof(Fruit).GetProperty (propertyName).SetValue (plantPartPanelManager.targetedFruit, value, null);
			break;
		}


		plantPartPanelManager.targetedPlant.InitializePlant ();
	}

	public void InitializeParameterList()
	{
		for (int i = 0; i < content.childCount; i++) {
			DisplayParameter d = content.GetChild (i).GetComponent<DisplayParameter> ();
			if (d != null) {
				d.Initialize (this);
				//Debug.Log ("init : " + d.name);
			}
		}

		contentParent.Init ();
	}
}
