using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
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

        // AudioManager is the default source if nothing is provided
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
            }
        }
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
                source.spatialBlend = soundData.spatialBlend;
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


}
