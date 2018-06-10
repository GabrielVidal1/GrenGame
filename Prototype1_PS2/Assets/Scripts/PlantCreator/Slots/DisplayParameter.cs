using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DisplayParameter : MonoBehaviour {





	public string parameterName;

	[SerializeField] protected TMP_Text parameterNameUI;

	private ParameterListManager parameterListManager;

	[SerializeField] private PlantPart concernedPart;

	/*
	[SerializeField] private bool hasConditionToAppear;
	[SerializeField] private BoolSlot[] andCondition, nandCondition;

	public void UpdateUI()
	{
		if (hasConditionToAppear) {
				

		}
	}
	*/
	public virtual object GetValue()
	{
		Debug.Log ("C'EST IMPOSSIBLE DE FAIRE CA");
		return null;
	}

	public virtual void Initialize(ParameterListManager parameterListManager)
	{
		this.parameterListManager = parameterListManager;

		parameterNameUI.text = DisplayParameter.NicifyName(parameterName);
	}

	public void OnValueChange()
	{
		if (parameterListManager != null) {
			parameterListManager.UpdatePlant (parameterName, GetValue(), concernedPart);

		}
	}

	public virtual void SetValue(object value)
	{

	}


	protected void UpdateParameterList()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());

	}

	public static string NicifyName(string variableName)
	{
		if (variableName.Length > 0) {
			string result = variableName [0].ToString ();
			for (int i = 1; i < variableName.Length; i++) {
				if ('A' <= variableName [i] && 'Z' >= variableName [i]) {
					result += " ";
				}
				result += variableName [i].ToString ();
			}
			return result;
		} else
			return "";
	}


}
