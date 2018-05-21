using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlantPartSelector : MonoBehaviour {

	[SerializeField] private Button trunkButton; 
	[SerializeField] private Button branchButton; 
	[SerializeField] private Button subBranchButton; 

	Button lastSelected;

	private PlantPart selection;
	public PlantPart SelectedPlantPart
	{
		get { return selection; }
	}

	public void ForceSelect(PlantPart part)
	{
		selection = part;
		switch(part)
		{
		case PlantPart.Trunk:
			SelectTrunk ();
			return;
		case PlantPart.Branch:
			SelectBranch();
			return;
		case PlantPart.SubBranch:
			SelectSubBranch();
			return;
		}
	}

	public void Initialize()
	{
		lastSelected = trunkButton;
		selection = PlantPart.Trunk;

		trunkButton.interactable = false;
		branchButton.interactable = true;
		subBranchButton.interactable = true;
	}

	public void SelectTrunk()
	{
		lastSelected.interactable = true;

		lastSelected = trunkButton;
		selection = PlantPart.Trunk;
		lastSelected.interactable = false;
	}

	public void SelectBranch()
	{
		lastSelected.interactable = true;

		lastSelected = branchButton;
		selection = PlantPart.Branch;
		lastSelected.interactable = false;
	}

	public void SelectSubBranch()
	{
		lastSelected.interactable = true;

		lastSelected = subBranchButton;
		selection = PlantPart.SubBranch;
		lastSelected.interactable = false;
	}

}
