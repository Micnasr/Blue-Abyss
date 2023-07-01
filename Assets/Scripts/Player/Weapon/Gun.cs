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
                // Create Hit on Effect
                Instantiate(hitOnEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
                Debug.Log(hitInfo[i].transform.name);

                // Grab FishHealthManager script
                FishHealthManager healthManager = hitInfo[i].transform.GetComponent<FishHealthManager>();

                // If script does not exist, try to get from parent
                if (healthManager == null)
                {
                    healthManager = hitInfo[i].transform.GetComponentInParent<FishHealthManager>();
                }

                if (healthManager != null)
                {
                    // Call TakeDamage function
                    healthManager.TakeDamage(gunData.damage);
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
