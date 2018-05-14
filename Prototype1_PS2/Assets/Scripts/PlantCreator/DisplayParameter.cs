using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DisplayParameter : MonoBehaviour {





	public string parameterName;

	[SerializeField] protected TMP_Text parameterNameUI;

	private int indexInPlantEditorManager;

	private PlantEditorGUI PEGUI;

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

		return null;
	}

	public virtual void Initialize(PlantEditorGUI PEGUI, int index)
	{
		this.PEGUI = PEGUI;
		indexInPlantEditorManager = index;


		parameterNameUI.text = NicifyName(parameterName);
	}

	public void OnValueChange()
	{
		if (PEGUI != null) {
			PEGUI.UpdatePlant (indexInPlantEditorManager, concernedPart);

		}
	}


	protected void UpdateParameterList()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());

	}

	public string NicifyName(string variableName)
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
