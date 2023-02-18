using System;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class Sound
{
    public enum AudioTypes { sfx, music }
    public AudioTypes audioType;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour {

    [SerializeField]
    public Sound[] sounds;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    private bool ready = false;
    
    public static AudioManager instance;
    void Awake() {
        
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

            switch (s.audioType)
            {
                case Sound.AudioTypes.sfx:
                    s.source.outputAudioMixerGroup = sfxMixerGroup;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }
        }

        ready = true;
    }

    private AudioSource GetSource(string name)
    {
        if (!ready) return null;
        
        Sound s = Array.Find(sounds, sound => sound.clip.name == name);
        if (s == null) throw new ArgumentException("Sound: " + name + " not found.");
        return s.source;
    }

    public void Play(string name) {
        AudioSource s = GetSource(name);
        if (s) s.Play();
    }

    public void Stop(string name)
    {
        AudioSource s = GetSource(name);
        if (s) s.Stop();
    }

    public float Length(string name)
    {
        AudioSource s = GetSource(name);
        return s ? s.clip.length : 0f;
    }
}
