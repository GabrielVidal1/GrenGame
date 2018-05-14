using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterListManager : MonoBehaviour {





	private DisplayParameter[] parametersSlots;
	private PlantPartPanelManager plantPartPanelManager;

	[SerializeField] private Transform content;

	public void Initialize(PlantPartPanelManager plantPartPanelManager)
	{
		this.plantPartPanelManager = plantPartPanelManager;
		InitializeParameterList ();
	}


	public void UpdatePlant(int parametersSlotsIndex, PlantPart pp)
	{
		string propertyName = parametersSlots [parametersSlotsIndex].parameterName;

		object value = parametersSlots [parametersSlotsIndex].GetValue();

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

		case PlantPart.FLower:
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

		parametersSlots = new DisplayParameter[content.childCount];

		int index = 0;
		for (int i = 0; i < content.childCount; i++) {
			DisplayParameter d = content.GetChild (i).GetComponent<DisplayParameter> ();
			if (d != null) {
				parametersSlots [index] = d;
				index += 1;
			}
		}


		for (int i = 0; i < index; i++) {
			parametersSlots [i].Initialize(this, i);
		}
	}
}
