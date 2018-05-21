using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class IntSlot : DisplayParameter {



	[SerializeField] private Slider slider;
	[SerializeField] private TMP_InputField inputField;

	[SerializeField] private bool hasSlider;

	[SerializeField] private int minValue;
	[SerializeField] private int maxValue;

	private int intValue;
	[SerializeField] private int defaultValue;

	public void SetDefaultValue(int value)
	{
		intValue = value;
	}


	public void UpdateValueSlider()
	{
		intValue = (int)slider.value;
		inputField.text = intValue.ToString ();
		OnValueChange ();
	}

	public override object GetValue ()
	{
		return intValue;
	}

	public override void Initialize(ParameterListManager parameterListManager)
	{
		base.Initialize (parameterListManager);

		intValue = defaultValue;
		slider.value = intValue;

		slider.minValue = minValue;
		slider.maxValue = maxValue;

		if (!hasSlider)
			slider.gameObject.SetActive (false);
	}

	public void UpdateValueInputField()
	{
		intValue = Mathf.Max (minValue, Mathf.Min (maxValue, int.Parse (inputField.text)));
		inputField.text = intValue.ToString ();
		slider.value = intValue;
		OnValueChange ();

	}
}
