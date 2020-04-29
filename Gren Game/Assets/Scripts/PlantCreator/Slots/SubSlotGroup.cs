using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SubSlotGroup : DisplayParameter {



	public override void Initialize (ParameterListManager parameterListManager)
	{
		base.Initialize (parameterListManager);

		float height = GetComponent<VerticalLayoutGroup> ().preferredHeight;

		Vector2 s = GetComponent<RectTransform> ().sizeDelta;
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (s.x, height);

		for (int i = 0; i < transform.childCount; i++) {
			DisplayParameter d = transform.GetChild (i).GetComponent<DisplayParameter> ();
			if (d != null) {
				d.Initialize(parameterListManager);
			}
		}



	}
}
