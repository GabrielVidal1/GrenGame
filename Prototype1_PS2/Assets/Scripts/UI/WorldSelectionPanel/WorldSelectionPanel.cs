using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using System.IO;

public class WorldSelectionPanel : MonoBehaviour {

	[SerializeField] private WorldButton worldButtonPrefab;

	[SerializeField] private WorldListContent worldListContent;

	[SerializeField] private TMP_InputField worldNameInputField;


	[SerializeField] private GameObject worldListPanel;
	[SerializeField] private GameObject newWorldPanel;

	[SerializeField] private GameObject multiplayerMenu;

	[SerializeField] private GameObject loadingScreen;
	[SerializeField] private Slider loadingScreenLoadingBar;


	void Start () 
	{
		FindWorlds ();
		
	}

	void FindWorlds()
	{
		string path = Application.persistentDataPath;

		if (!Directory.Exists (path + "/Worlds")) {
			Directory.CreateDirectory(path + "/Worlds");
			//Debug.Log (path + "/Worlds");
		}

		string[] worldFilesPathes = Directory.GetFiles (path + "/Worlds/");

		int validWorlds = 0;
		foreach (string worldFilePath in worldFilesPathes) 
		{
			//Debug.Log (worldFilePath.Substring (worldFilePath.Length - 10, 10));
			if (worldFilePath.Substring (worldFilePath.Length - 10, 10) == ".grenworld") {

				//Debug.Log (worldFilePath + " is a valid world !");

				WorldButton wb = Instantiate (worldButtonPrefab, worldListContent.transform).GetComponent<WorldButton> ();


				string worldName = worldFilePath.Substring (worldFilePath.LastIndexOf ("/") + 1, worldFilePath.LastIndexOf (".") - worldFilePath.LastIndexOf ("/") - 1);
				//Debug.Log (worldName);

				wb.Initialize (worldName);

				validWorlds++;
			}
		}
		worldListContent.SetHeight (validWorlds, worldButtonPrefab.GetComponent<RectTransform> ().rect.height);

	}


	public void NewWorldPanel()
	{
		worldListPanel.SetActive (false);
		newWorldPanel.SetActive (true);
	}

	public void FromNewWorldPanelToWorldListPanel()
	{
		worldListPanel.SetActive (true);
		newWorldPanel.SetActive (false);
	}

	public void CreateNewWorld()
	{
		Debug.Log ("clicked on create new world button");
		loadingScreen.SetActive (true);
		GameManager.gm.LaunchNewWorld (worldNameInputField.text, loadingScreenLoadingBar);
	}

	public void BackToMultiplayerMenu()
	{
		multiplayerMenu.SetActive (true);
		gameObject.SetActive (false);
	}


	public void Launch(string worldName)
	{
		Debug.Log (gameObject.activeSelf);
		GameManager.gm.SetWorld (worldName);

		loadingScreen.SetActive (true);
		worldListPanel.SetActive (false);

		gameObject.SetActive (true);

		Debug.DebugBreak ();

		GameManager.gm.LoadScene (loadingScreenLoadingBar, GameManager.gm.mainSceneName);



	}
	/*
	public IEnumerator LoadMainScene()
	{
		

		AsyncOperation loading = SceneManager.LoadSceneAsync(GameManager.gm.mainSceneName);
		loading.allowSceneActivation = false;

		Debug.Log ("loading started !");

		while (!loading.isDone) {
			yield return null;

			//Debug.Log ("loading progress : " + loading.progress);


			loadingScreenLoadingBar.value = loading.progress;
		}

		GameManager.gm.Launch ();
		Debug.Break ();
		loading.allowSceneActivation = true;
	}
	*/
}
