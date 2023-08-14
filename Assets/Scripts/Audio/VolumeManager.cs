using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private float globalVolume = 1f;
    public Slider volumeSlider;

    private void Awake()
    {
        globalVolume = PlayerPrefs.GetFloat("GlobalVolume", globalVolume);
        volumeSlider.value = globalVolume;
    }

    public void ChangeGlobalVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat("GlobalVolume", newVolume);
        PlayerPrefs.Save();
    }
}
