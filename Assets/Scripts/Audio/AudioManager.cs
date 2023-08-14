using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // NEVER TOUCH TO AVOID LOSING DATA
    public Sound[] sounds;

    public static AudioManager instance;

    private string currentMusic;
    
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

        // Give each 2D sound a component under AudioManager 
        foreach(Sound s in sounds)
        {
            if (s.spatialBlend == 0)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
            }
        }
    }

    private void Start()
    {
        // Play Starting Music
        Play("SurfaceMusic");
        currentMusic = "SurfaceMusic";
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

    // Can only use this function for sounds with unique gameobjects! (2D Sounds ONLY)
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

    public void FadeTrack(string nextName, float fadeDuration)
    {
        Sound currentSoundData = Array.Find(sounds, sound => sound.name == currentMusic);
        Sound nextSoundData = Array.Find(sounds, sound => sound.name == nextName);

        AudioSource currentSource = FindAudioSourceWithClip(gameObject, currentSoundData.clip);
        AudioSource nextSource = FindAudioSourceWithClip(gameObject, nextSoundData.clip);

        currentMusic = nextName;

        StartCoroutine(FadeOutTrack(currentSource, fadeDuration));
        StartCoroutine(FadeInTrack(nextSource, fadeDuration, nextSoundData.volume));
    }

    private IEnumerator FadeOutTrack(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            float elapsed = Time.time - startTime;
            float normalizedTime = elapsed / fadeDuration;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, normalizedTime);
            yield return null;
        }

        audioSource.volume = 0;
    }

    private IEnumerator FadeInTrack(AudioSource audioSource, float fadeDuration, float targetVolume)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float startVolume = 0f;
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            float elapsed = Time.time - startTime;
            float normalizedTime = elapsed / fadeDuration;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, normalizedTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
