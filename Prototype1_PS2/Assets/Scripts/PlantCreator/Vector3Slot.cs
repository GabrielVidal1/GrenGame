using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Vector3Slot : DisplayParameter {

	[SerializeField] private TMP_InputField inputFieldX;
	[SerializeField] private TMP_InputField inputFieldY;
	[SerializeField] private TMP_InputField inputFieldZ;

	[SerializeField] private bool cantBeZero;

	[SerializeField] private Vector3 vector3Value;

	[SerializeField] private Vector3 defaultValue;

	//[SerializeField] private Vector3 defaultValue;


	public void SetDefaultValue(Vector3 value)
	{
		vector3Value = value;
	}

	public override object GetValue ()
	{
		return vector3Value;
	}

	public override void Initialize(ParameterListManager parameterListManager)
	{
		base.Initialize (parameterListManager);

		vector3Value = defaultValue;

		inputFieldX.text = vector3Value.x.ToString ();
		inputFieldY.text = vector3Value.y.ToString ();
		inputFieldZ.text = vector3Value.z.ToString ();
	}


	public void UpdateValueFields()
	{
		float x = float.Parse (inputFieldX.text);
		float y = float.Parse (inputFieldY.text);
		float z = float.Parse (inputFieldZ.text);

		vector3Value = new Vector3 (x, y, z);

		if (vector3Value == Vector3.zero && cantBeZero) {
			vector3Value = defaultValue;

			inputFieldX.text = vector3Value.x.ToString ();
			inputFieldY.text = vector3Value.y.ToString ();
			inputFieldZ.text = vector3Value.z.ToString ();
		}

		OnValueChange ();

	}

}
