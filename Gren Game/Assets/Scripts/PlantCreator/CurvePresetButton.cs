using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvePresetButton : MonoBehaviour {


	public AnimationCurve preset;


	CurveZone parent;

	public void Initialize(CurveZone parent)
	{
		this.parent = parent;
	}

	public void OnClick()
	{
		parent.SetCurve (preset);
	}
}
