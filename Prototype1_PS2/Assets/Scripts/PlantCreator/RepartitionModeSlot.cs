using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RepartitionModeSlot : DisplayParameter {

	[SerializeField] private LeavesRepartitionMode repartitionModeValue;

	[SerializeField] private LeavesRepartitionMode defaultValue;

	[SerializeField] private Button numberButton, densityButton;

	[SerializeField] private DisplayParameter[] densitySlots, numberSlots;

	public override void Initialize (ParameterListManager parameterListManager, int index)
	{
		base.Initialize (parameterListManager, index);

		repartitionModeValue = defaultValue;
		TurnOff ();

	}

	public void TurnOn()
	{
		if (repartitionModeValue == LeavesRepartitionMode.Number) {
			numberButton.interactable = false;
			densityButton.interactable = true;
			for (int i = 0; i < densitySlots.Length; i++) {
				densitySlots [i].gameObject.SetActive (false);
			}
			for (int i = 0; i < numberSlots.Length; i++) {
				numberSlots [i].gameObject.SetActive (true);
			}
		} else {
			numberButton.interactable = true;
			densityButton.interactable = false;
			for (int i = 0; i < densitySlots.Length; i++) {
				densitySlots [i].gameObject.SetActive (true);
			}
			for (int i = 0; i < numberSlots.Length; i++) {
				numberSlots [i].gameObject.SetActive (false);
			}
		}
	}

	public void TurnOff()
	{
		for (int i = 0; i < densitySlots.Length; i++) {
			densitySlots [i].gameObject.SetActive (false);
		}
		for (int i = 0; i < numberSlots.Length; i++) {
			numberSlots [i].gameObject.SetActive (false);
		}
	}

	public override object GetValue ()
	{
		return repartitionModeValue;
	}

	public void SetToNumber()
	{
		numberButton.interactable = false;
		densityButton.interactable = true;

		repartitionModeValue = LeavesRepartitionMode.Number;

		for (int i = 0; i < densitySlots.Length; i++) {
			densitySlots [i].gameObject.SetActive (false);
		}
		for (int i = 0; i < numberSlots.Length; i++) {
			numberSlots [i].gameObject.SetActive (true);
		}

		OnValueChange ();
	}

	public void SetToDensity()
	{
		numberButton.interactable = true;
		densityButton.interactable = false;

		repartitionModeValue = LeavesRepartitionMode.Density;

		for (int i = 0; i < densitySlots.Length; i++) {
			densitySlots [i].gameObject.SetActive (true);
		}
		for (int i = 0; i < numberSlots.Length; i++) {
			numberSlots [i].gameObject.SetActive (false);
		}

		OnValueChange ();
	}
}
