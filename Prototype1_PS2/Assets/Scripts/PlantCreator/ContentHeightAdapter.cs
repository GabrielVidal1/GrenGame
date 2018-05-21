using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ContentHeightAdapter : MonoBehaviour {

	public void Init()
	{
		Vector2 s = GetComponent<RectTransform> ().sizeDelta;
		float h = transform.GetChild (0).GetComponent<VerticalLayoutGroup> ().preferredHeight;

		//GetComponent<RectTransform> ().sizeDelta = new Vector2 (s.x, h);
	}
}
