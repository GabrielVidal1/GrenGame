using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour {

	[SerializeField] private OpenPanelSlot openPanelSlotPrefab;
	[SerializeField] PlantPartPanelManager plantPartPanelManager;

	[SerializeField] private Transform plantListParent;
	[SerializeField] private PlantSerializer plantSerializer;
	[SerializeField] private FileMenu fileMenu;

	public void Open()
	{
		string mPath = Application.persistentDataPath + "/Plants/";

		string[] paths = System.IO.Directory.GetFiles (mPath, "*.grenplant");

		for (int i = 0; i < plantListParent.childCount; i++)
			Destroy (plantListParent.GetChild (i).gameObject);
		

		for (int i = 0; i < paths.Length; i++) {
			string path = paths [i];

			string plantName = path.Substring (path.LastIndexOf ("/") + 1, path.Length - path.LastIndexOf ("/") - 11);

			OpenPanelSlot s = (OpenPanelSlot)Instantiate (openPanelSlotPrefab, plantListParent);
			s.Initialize (this, path, plantName);
		}
	}

	public void LoadPlant(string path, string plantName)
	{
		print (plantName);
		plantSerializer.UnserializePlant (path, plantPartPanelManager);
		fileMenu.SetName (plantName);
		Close ();
	}

	public void Close()
	{
		gameObject.SetActive (false);
	}
}
