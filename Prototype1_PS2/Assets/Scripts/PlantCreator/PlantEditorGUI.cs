using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlantEditorGUI : MonoBehaviour {



	[SerializeField] private Plant targetedPlant;
	[SerializeField] private Plant targetedBranch;
	[SerializeField] private Plant targetedSubBranch;
	[SerializeField] private Leaf targetedLeaf;
	[SerializeField] private Flower targetedFlower;
	[SerializeField] private Fruit targetedFruit;

	//[SerializeField] private Transform propertyList;

	[SerializeField] private RectTransform parametersSlotsParent;

	/*
	[Header("Slot Prefabs")]

	[SerializeField] private IntSlot intSlotPrefab;
	[SerializeField] private FloatSlot floatSlotPrefab;
	[SerializeField] private CurveSlot curveSlotPrefab;
	[SerializeField] private IntervalSlot intervalSlotPrefab;
	[SerializeField] private Vector3Slot vector3SlotPrefab;
	[SerializeField] private BoolSlot boolSlotPrefab;
*/
	/*
	[Header("Slots")]

	private DisplayParameter[] parametersSlots;

	// Use this for initialization
	void Start () {
		InitializeParameterList ();
	}



	public void UpdatePlant(int parametersSlotsIndex, PlantPart pp)
	{
		string propertyName = parametersSlots [parametersSlotsIndex].parameterName;

		object value = parametersSlots [parametersSlotsIndex].GetValue();

		switch (pp) {
		case PlantPart.Trunk:
			typeof(Plant).GetProperty (propertyName).SetValue (targetedPlant, value, null);
			break;

		case PlantPart.Branch:
			typeof(Plant).GetProperty (propertyName).SetValue (targetedBranch, value, null);
			break;

		case PlantPart.SubBranch:
			typeof(Plant).GetProperty (propertyName).SetValue (targetedSubBranch, value, null);
			break;

		case PlantPart.Leaf:
			typeof(Leaf).GetProperty (propertyName).SetValue (targetedLeaf, value, null);
			break;

		case PlantPart.FLower:
			typeof(Flower).GetProperty (propertyName).SetValue (targetedFlower, value, null);
			break;

		case PlantPart.Fruit:
			typeof(Fruit).GetProperty (propertyName).SetValue (targetedFruit, value, null);
			break;
		}


		targetedPlant.InitializePlant ();
	}


	public void InitializeParameterList()
	{
		parametersSlots = new DisplayParameter[parametersSlotsParent.childCount];

		int index = 0;
		for (int i = 0; i < parametersSlotsParent.childCount; i++) {
			DisplayParameter d = parametersSlotsParent.GetChild (i).GetComponent<DisplayParameter> ();
			if (d != null) {
				parametersSlots [index] = d;
				index += 1;
			}
		}


		for (int i = 0; i < index; i++) {
			parametersSlots [i].Initialize(this, i);
		}
	}
	*/
}

public enum PlantPart
{
	Trunk,
	Branch,
	SubBranch,
	Leaf,
	Flower,
	Fruit
}