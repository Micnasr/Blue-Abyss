using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSounds : MonoBehaviour
{
    public void GunShotSound()
    {
        FindObjectOfType<AudioManager>().Play("shotgunShoot");
    }

    public void GunCockingSound()
    {
        FindObjectOfType<AudioManager>().Play("shotgunCocking");
    }
}
