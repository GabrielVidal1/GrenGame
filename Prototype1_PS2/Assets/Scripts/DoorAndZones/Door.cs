using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Door : MonoBehaviour {

    public AudioClip open_sound;
    public AudioClip close_sound;
    public AudioClip locked_sound;

    public AudioSource audioSource;

    private bool soundHasBeenPlayed = false;

	public Zone[] associatedZones;

	[SerializeField] private int neededPointsToOpen;

	[SerializeField] private Animator doorAnimator;

	[SerializeField] private Slider slider;

	[SerializeField] private bool withoutSlider;

	[SerializeField]
	private bool opened;
	public bool IsOpen
	{
		get {return opened;}
	}

	void Start () 
	{
		foreach (Zone zone in associatedZones) {
			zone.AddToDoorArray (this);
		}
		if (withoutSlider)
			slider.gameObject.SetActive (false);
	}

	public void InitOpen()
	{
		opened = true;
		doorAnimator.SetTrigger ("InitOpen");
		doorAnimator.SetBool ("Opened", true);

	}


	private void Open()
	{
        doorAnimator.SetBool("Opened", true);
        audioSource.clip = open_sound;
        audioSource.Play();
        opened = true;
	}

	private void Close()
	{
		doorAnimator.SetBool ("Opened", false);
        audioSource.clip = close_sound;
        audioSource.Play();
        opened = false;
	}

	public void Interact()
	{
        if (CanOpen() && !opened) {
			Open ();
		} else {
			Close ();
		}
	}

	public bool CanOpen()
	{
		return TotalPoint() >= neededPointsToOpen || neededPointsToOpen == 0;
	}

	public void UpdateSlider()
	{
		int tot = TotalPoint ();

        //Debug.Log (tot);

        if (tot > neededPointsToOpen)
        {
            if (!soundHasBeenPlayed)
            {
                audioSource.clip = locked_sound;
                audioSource.Play();
                soundHasBeenPlayed = true;
            }
            slider.value = 1f;
        }
        else
            slider.value = (float)tot / neededPointsToOpen;
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
