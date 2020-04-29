using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreenPanel : MonoBehaviour {

	[SerializeField] private string mainSceneName;

	AsyncOperation op;

	void Start () {
		StartCoroutine(LoadMainScene ());
	}
	public void LoadScene()
	{
		op.allowSceneActivation = true;
	}

	public IEnumerator LoadMainScene()
	{
		op = SceneManager.LoadSceneAsync (mainSceneName);
		op.allowSceneActivation = false;

		while (!op.isDone) {
			yield return null;
		}
		

	}
}
