using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Header("General Settings")]
    [Range(0f,1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [Header("3D Audio Settings")]
    [Range(0f, 1f)]
    public float spatialBlend;

    public bool logarithmicRolloff;

    public float maxDistance;

    [HideInInspector]
    public AudioSource source;
}
