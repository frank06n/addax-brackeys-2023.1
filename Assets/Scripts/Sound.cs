using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {
    public enum AudioTypes {sfx, music}
    public AudioTypes audioType;

    public AudioClip clip;

    public string name;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;
    
    [HideInInspector]
    public AudioSource source;

}
