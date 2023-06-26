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

            RaycastHit[] hitInfo;

            // Raycast Goes Through Everything
            hitInfo = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.forward, gunData.maxDistance);

            for (int i = 0; i < hitInfo.Length; i++)
            {
                if (!hitInfo[i].transform.CompareTag("WaterCollider"))
                {
                    // Create Hit on Effect
                    Instantiate(hitOnEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
                    Debug.Log(hitInfo[i].transform.name);
                }
            }

            timeSinceLastShot = 0;

            // Play Animation
            spoolAnimator.Play("BasicHarpoonShoot", 0, 0);
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }
}
