using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMainMenu : MonoBehaviour {


	[SerializeField] private float transitionDuration;

	[SerializeField] private bool canvasGroupOrFadePanel;

	[SerializeField] private RawImage fadePanel;
	[SerializeField] private CanvasGroup canvasGroup;

	public delegate void Transition();
	public Transition transition;


	public IEnumerator FadeOn()
	{
		for (float i = 0f; i <= 1f; i += Time.deltaTime / transitionDuration) {
			if (canvasGroupOrFadePanel)
				canvasGroup.alpha = 1f - i;
			else 
				fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, i);
			yield return null;
		}
		if (canvasGroupOrFadePanel)
			canvasGroup.alpha = 0f;
		else
			fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1f);
	}

	public IEnumerator FadeOff()
	{
		for (float i = 0f; i <= 1f; i += Time.deltaTime / transitionDuration) {
			if (canvasGroupOrFadePanel)
				canvasGroup.alpha = i;
			else
				fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1f - i);
			yield return null;
		}
		if (canvasGroupOrFadePanel)
			canvasGroup.alpha = 1f;
		else
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
