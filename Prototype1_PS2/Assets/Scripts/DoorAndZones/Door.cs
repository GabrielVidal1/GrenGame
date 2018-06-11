using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Door : MonoBehaviour {

    #region sounds
    public AudioClip open_sound;
    public AudioClip close_sound;
    public AudioClip locked_sound;

    public AudioSource doorsoundSource;
    public AudioSource insectsoundSource;
    private bool dingHasBeenPlayed = false;
    #endregion

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

    void Awake()
    {
        insectsoundSource.volume = 0f;
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
        doorsoundSource.clip = open_sound;
        doorsoundSource.Play();
        opened = true;
	}

	private void Close()
	{
		doorAnimator.SetBool ("Opened", false);
        doorsoundSource.clip = close_sound;
        doorsoundSource.Play(22000);
        opened = false;
	}

	public void Interact()
	{
        if (!CanOpen())
        {
            doorsoundSource.clip = locked_sound;
            doorsoundSource.Play();
        }
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
            if (!dingHasBeenPlayed)
            {
                doorsoundSource.clip = locked_sound;
                doorsoundSource.Play();
                dingHasBeenPlayed = true;
            }
            slider.value = 1f;
        }
        else
            insectsoundSource.volume = slider.value = (float)tot / neededPointsToOpen;

        if (tot >= neededPointsToOpen / 2)
        {
            FindObjectOfType<AudioManager>().playForest = true;
        }
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
