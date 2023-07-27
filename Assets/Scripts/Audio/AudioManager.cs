using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // NEVER TOUCH TO AVOID LOSING DATA
    public Sound[] sounds;

    public static AudioManager instance;
    
    void Awake()
    {
        // For Multiple Scenes
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(string name, float pitch = 1f, GameObject sourceGameObject = null)
    {
        Sound soundData = Array.Find(sounds, sound => sound.name == name);

        if (soundData == null)
        {
            Debug.LogWarning(name + " Audio Not FOUND");
            return;
        }

        AudioSource source = null;

        if (sourceGameObject != null)
        {
            source = FindAudioSourceWithClip(sourceGameObject, soundData.clip);
            if (source == null)
            {
                source = sourceGameObject.AddComponent<AudioSource>();
                source.clip = soundData.clip;
                source.volume = soundData.volume;
                source.pitch = soundData.pitch;
                source.loop = soundData.loop;
                source.spatialBlend = soundData.spatialBlend;
                source.maxDistance = soundData.maxDistance;

                // Either we choose logarithmic or Linear 
                if (soundData.logarithmicRolloff)
                    source.rolloffMode = AudioRolloffMode.Logarithmic;
                else
                    source.rolloffMode = AudioRolloffMode.Linear;

                soundData.source = source;
            }
        }
        // AudioManager is the default source if nothing is provided
        else
        {
            source = FindAudioSourceWithClip(gameObject, soundData.clip);
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                source.clip = soundData.clip;
                source.volume = soundData.volume;
                source.pitch = soundData.pitch;
                source.loop = soundData.loop;

                soundData.source = source;
            }
        }


        source.pitch = pitch;

        source.Play();
    }

    private AudioSource FindAudioSourceWithClip(GameObject obj, AudioClip clip)
    {
        AudioSource[] audioSources = obj.GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            if (source.clip == clip)
            {
                return source;
            }
        }
        return null;
    }

    // Can only use this function for sounds with unique gameobjects!
    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }
}
