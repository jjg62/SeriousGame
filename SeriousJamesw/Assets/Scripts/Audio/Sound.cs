using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object encapsulating data about a sound effect
//Serializable so can be setup in inspector
[System.Serializable]
public class Sound
{
    public string name; //Name used to search for sound
    public AudioClip clip; //Sound file

    [HideInInspector]
    public AudioSource source; //Connected AudioSource component - this actually handles the sound playing in the engine

    [Range(0,1)]
    public float volume;
    public float pitch;
    public bool loop;
}
