using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMainMenu : MonoBehaviour {


	[SerializeField] private float transitionDuration;

	[SerializeField] private RawImage fadePanel;

	public delegate void Transition();
	public Transition transition;


	public IEnumerator FadeOn()
	{
		for (float i = 0f; i <= 1f; i += Time.deltaTime / transitionDuration) {
			fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, i);
			yield return null;
		}

		fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1f);
	}

	public IEnumerator FadeOff()
	{
		for (float i = 0f; i <= 1f; i += Time.deltaTime / transitionDuration) {
			fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1f - i);
			yield return null;
		}

		fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0f);
	}

	public void Transit()
	{
		StartCoroutine (TransitCoroutine ());
	}

	IEnumerator TransitCoroutine()
	{
		yield return StartCoroutine (FadeOn ());

		transition ();

		yield return StartCoroutine (FadeOff ());
	}
}
