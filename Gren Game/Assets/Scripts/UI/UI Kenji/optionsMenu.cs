﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class optionsMenu : MonoBehaviour {

    public static float musicVolume = 0.5F;

	public bool noTransition;

    public Slider volumeSlider;

    public GameObject menu;
    public GameObject parent;

	[SerializeField] private MainMainMenu mainMainMenu;

    // Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	/*
	void Update () {
        if (isActiveAndEnabled && Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }*/

    public void ToggleMusic(bool on)
    {
        if (on)
        {
            Debug.Log("Music OFF");
            AudioListener.volume = 0;
        }
        else
        {
            Debug.Log("Music ON");
            AudioListener.volume = musicVolume;
        }
    }

    public void ChangeVolume()
    {
        Debug.Log("Changed volume to " + volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
    }

    public void LoadMenu()
    {
        menu.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

	public void Back()
	{
		if (!noTransition) {
			mainMainMenu.transition = VoidBack;
			mainMainMenu.Transit ();
		} else {
			VoidBack ();
		}
	}

	void VoidBack()
    {
        menu.SetActive(false);
        parent.SetActive(true);
    }

    public void Fullscreen(bool isFc)
    {
        Screen.fullScreen = isFc;
    }
}