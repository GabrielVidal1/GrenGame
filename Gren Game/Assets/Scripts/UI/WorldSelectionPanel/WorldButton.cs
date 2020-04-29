using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WorldButton : MonoBehaviour {

	[SerializeField] private TMP_Text worldNameText;

	[SerializeField] private Animator settingsIconAnimator;

	//[SerializeField] private WorldSelectionPanel worldSelectionPanel;


	WorldSelectionPanel worldSelectionPanel;

	Animator animator;

	public void OpenSettingsPanel()
	{
		worldSelectionPanel.ResetLastSelectedWorldSettings ();
		worldSelectionPanel.SetLastSelectedWorldSettings (this);


		settingsIconAnimator.SetTrigger ("Disabled");
		settingsIconAnimator.GetComponent<Button> ().interactable = false;

		animator.SetBool ("Open Settings", true);
	}

	public void DeleteThisWorld()
	{
		worldSelectionPanel.DeleteWorld (worldName);
	}

	public void RenameThisWorld()
	{
		worldSelectionPanel.RenameWorld (worldName);

	}

	private string worldName;

	public void Reset()
	{
		settingsIconAnimator.GetComponent<Button> ().interactable = true;
		animator.SetBool ("Open Settings", false);


	}

	public void Initialize(string wn, WorldSelectionPanel wsp)
	{
		worldName = wn;
		worldNameText.text = wn;
		worldSelectionPanel = wsp;

		animator = GetComponent<Animator> ();
	}

	public void LauchWorld()
	{

		Debug.Log(transform.parent.parent.parent.parent.parent.name);
		transform.parent.parent.parent.parent.parent.GetComponent<WorldSelectionPanel> ().Launch (worldName);
	}

}
