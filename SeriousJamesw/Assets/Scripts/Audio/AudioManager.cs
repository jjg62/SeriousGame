using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Code than can be called statically to play audio
//Some code taken from tutorial: https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour
{
    //Setup sounds in inspector
    [SerializeField]
    private Sound[] sounds;

    //Singleton pattern
    public static AudioManager instance;

    private void Awake()
    {
        //Allow gameObject to persist on scene change
        DontDestroyOnLoad(gameObject);

        //Ensure there is always exactly one instance with this script
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //For each sound in inspector, make a new AudioSource component
        //and attach it to the Sound object using Sound.source
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    //Play the sound named soundName
    public void Play(string soundName)
    {
        //Look for the sound in sounds
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        //Only play sound if found in array
        if (s != null && !s.source.isPlaying) s.source.Play();
    }

    //Stop sound name soundName
    public void Stop(string soundName)
    {
        //Look for the sound in sounds
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        //If found, stop
        if (s != null) s.source.Stop();
    }

    //Set volume of sound 
    public void SetVolume(string name, float vol)
    {
        //Look for the sound in sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            //If found, change volume value in inspector and of the AudioSource
            s.volume = vol;
            s.source.volume = vol;
        }
    }

}
