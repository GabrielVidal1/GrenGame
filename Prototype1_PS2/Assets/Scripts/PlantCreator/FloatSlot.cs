using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class FloatSlot : DisplayParameter {

	[SerializeField] private Slider slider;
	[SerializeField] private TMP_InputField inputField;

	[SerializeField] private bool hasSlider;

	[SerializeField] private float minValue;
	[SerializeField] private float maxValue;

	private float floatValue;

	[SerializeField] private float defaultValue;

	public void SetDefaultValue(float value)
	{
		floatValue = value;
	}

	public void UpdateValueSlider()
	{
		floatValue = slider.value;
		inputField.text = floatValue.ToString ();
		OnValueChange ();

	}

	public override object GetValue ()
	{
		return floatValue;
	}

	public override void Initialize(ParameterListManager parameterListManager)
	{
		base.Initialize (parameterListManager);

		floatValue = defaultValue;
		slider.value = floatValue;

		slider.minValue = minValue;
		slider.maxValue = maxValue;

		//Debug.Log ("min : " + minValue + "   max:" + maxValue);

		if (!hasSlider)
			slider.gameObject.SetActive (false);
	}

	public void UpdateValueInputField()
	{
		floatValue = Mathf.Max (minValue, Mathf.Min (maxValue, float.Parse (inputField.text)));
		inputField.text = floatValue.ToString ();
		slider.value = floatValue;
		OnValueChange ();

	}
}
