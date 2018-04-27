using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class optionsMenu : MonoBehaviour {

    public static bool musicON = true;

    public static float musicVolume = 0.5F;

    public Slider volumeSlider;
    public GameObject textMusicON;
    public GameObject textMusicOFF;

    public GameObject menu;
    public GameObject parent;

    // Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (isActiveAndEnabled && Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void ToggleMusic()
    {
        if (musicON)
        {
            Debug.Log("Music OFF");
            musicON = false;
            AudioListener.volume = 0;
            textMusicOFF.SetActive(true);
            textMusicON.SetActive(false);
        }
        else
        {
            Debug.Log("Music ON");
            musicON = true;
            AudioListener.volume = musicVolume;
            textMusicOFF.SetActive(false);
            textMusicON.SetActive(true);
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
        menu.SetActive(false);
        parent.SetActive(true);
    }

    public void Fullscreen(bool isFc)
    {
        Screen.fullScreen = isFc;
    }
}