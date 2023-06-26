using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;
    public GameObject hitOnEffect;

    public Transform muzzle;

    public Camera playerCam;

    public Animator spoolAnimator;

    float timeSinceLastShot;

    private void Start()
    {
        //Subscribing to Event
        PlayerShoot.shootInput += Shoot;
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if (CanShoot())
        {

            RaycastHit hitInfo;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitInfo, gunData.maxDistance))
            {
                Debug.Log(hitInfo.transform.name);
            }

            timeSinceLastShot = 0;

            spoolAnimator.Play("BasicHarpoonShoot", 0, 0);

            Instantiate(hitOnEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }
}
