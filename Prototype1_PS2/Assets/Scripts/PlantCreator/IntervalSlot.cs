using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class IntervalSlot : DisplayParameter {

	[SerializeField] private TMP_InputField inputFieldmin;
	[SerializeField] private TMP_InputField inputFieldmax;

	[SerializeField] private Interval intervalValue;

	[SerializeField] private Interval defaultValue;

	public void SetDefaultValue(Interval value)
	{
		intervalValue = value;
	}

	public override object GetValue ()
	{
		return intervalValue;
	}

	public override void Initialize(ParameterListManager parameterListManager)
	{
		base.Initialize (parameterListManager);

		intervalValue = defaultValue;
		OnValueChange ();

		inputFieldmin.text = intervalValue.min.ToString ();
		inputFieldmax.text = intervalValue.max.ToString ();;
	}

	public void UpdateValue()
	{
		intervalValue.min =  float.Parse (inputFieldmin.text);
		intervalValue.max = float.Parse (inputFieldmax.text);

		OnValueChange ();
	}

}
