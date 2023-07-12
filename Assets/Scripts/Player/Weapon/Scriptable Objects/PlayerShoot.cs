using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootInput?.Invoke();
        }
    }
}
