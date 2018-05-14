using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BoolSlot : DisplayParameter {

	[SerializeField] private Toggle toggle;
	private bool boolValue;
	[SerializeField] private bool defaultValue;

	[SerializeField] private DisplayParameter[] dependentSlots;

	public void SetDefaultValue(bool value)
	{
		boolValue = value;
	}

	public override object GetValue ()
	{
		return boolValue;
	}

	public override void Initialize (ParameterListManager parameterListManager, int index)
	{
		base.Initialize (parameterListManager, index);
		boolValue = defaultValue;

		toggle.isOn = boolValue;

		for (int i = 0; i < dependentSlots.Length; i++) {
			dependentSlots [i].gameObject.SetActive (boolValue);
		}

		UpdateValue ();
	}

	public void UpdateValue()
	{

		boolValue = toggle.isOn;
		OnValueChange ();

		for (int i = 0; i < dependentSlots.Length; i++) {
			dependentSlots [i].gameObject.SetActive (boolValue);
		}

	}

}
