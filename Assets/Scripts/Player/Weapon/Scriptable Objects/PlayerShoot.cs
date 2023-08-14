using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    private Gun gun;

    private void Start()
    {
        gun = FindAnyObjectByType<Gun>();
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            shootInput?.Invoke();
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            gun.Shoot();
        }
    }
}
