using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {





	public Zone[] associatedZones;

	[SerializeField] private int neededPointsToOpen;

	[SerializeField] private Animator doorAnimator;


	private bool opened;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Open()
	{
		doorAnimator.SetBool ("Opened", true);
		opened = true;
	}

	private void Close()
	{
		doorAnimator.SetBool ("Opened", false);
		opened = false;
	}

	public void Interact()
	{
		if (CanOpen () && !opened) {
			Open ();
		} else {
			Close ();
		}
	}

	public bool CanOpen()
	{
		return TotalPoint() >= neededPointsToOpen;
	}

	public void UpdateSlider()
	{
		

	}

	private int TotalPoint()
	{
		int sum = 0;

		foreach (Zone zone in associatedZones) {
			sum += zone.TotalPoints ();
		}

		return sum;
	}

}
