using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSounds : MonoBehaviour
{
    public void GunShotSound()
    {
        float randomPitch = Random.Range(0.9f, 1.2f);
        FindObjectOfType<AudioManager>().Play("shotgunShoot", randomPitch);
    }

    public void GunCockingSound()
    {
        FindObjectOfType<AudioManager>().Play("shotgunCocking");
    }
}
