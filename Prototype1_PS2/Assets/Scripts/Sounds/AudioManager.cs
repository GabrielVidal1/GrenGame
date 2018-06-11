using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public bool playForest = false;
    private System.Random rnd = new System.Random();

    // Pour jouer un son : FindObjectOfType<AudioManager>().Play(sound_name)

	void Awake () {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        Sound foret_ambiance = Array.Find(sounds, sound => sound.name == "foret_ambiance");
        foret_ambiance.source.playOnAwake = true;
        foret_ambiance.source.loop = true;
        foret_ambiance.source.mute = true;
	}

    void Update()
    {
        if (playForest)
        {
            Array.Find(sounds, sound => sound.name == "foret_ambiance").source.mute = false;
            if (rnd.Next(800) == 0)
            {
                Play("foret_oiseau");
            }
        }
        
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
