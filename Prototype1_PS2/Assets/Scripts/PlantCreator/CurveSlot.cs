using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSlot : DisplayParameter {


	[SerializeField] private AnimationCurve curveValue;
	public AnimationCurve CurveValue 
	{get { return curveValue; }set{ curveValue = value; }}

	[SerializeField] private float closedHeight;
	[SerializeField] private float openedHeight;

	[SerializeField] private GameObject curveBox;

	[SerializeField] private CurveZone curveZone;

	private RectTransform rectTransform;
	private bool opened = false;

	public void SetDefaultValue(AnimationCurve value)
	{
		curveValue = value;
	}


	public void Open()
	{
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, openedHeight);
		UpdateParameterList ();
		curveBox.SetActive (true);

		curveZone.Initialize ();
	}

	public void Close()
	{
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, closedHeight);
		UpdateParameterList ();
		curveBox.SetActive (false);

	}

	public void ToggleOpenClose()
	{
		opened = !opened;

		if (opened)
			Open ();
		else
			Close ();
	}


	public override void Initialize (PlantEditorGUI PEGUI, int index)
	{
		base.Initialize (PEGUI, index);
		rectTransform = GetComponent<RectTransform> ();

		curveBox.SetActive (true);
		curveZone.Initialize ();
		curveZone.UpdateCurve ();
		curveBox.SetActive (false);
	}

	public override object GetValue ()
	{
		return curveValue;
	}
	

	public void UpdateValue()
	{
		OnValueChange ();
	}
}
