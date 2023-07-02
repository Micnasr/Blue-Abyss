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

            List<GameObject> hitObjects = new List<GameObject>(); // List to store already hit objects

            for (int i = 0; i < hitInfo.Length; i++)
            {

                GameObject hitObject = hitInfo[i].transform.gameObject;

                // Check if the same object has been hit before
                if (hitObjects.Contains(hitObject))
                {
                    // Skip so we dont apply damage twice
                    continue;
                }

                // Create Hit on Effect
                Instantiate(hitOnEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
                Debug.Log(hitInfo[i].transform.name);

                // Add the hit object to the list
                hitObjects.Add(hitObject);

                // Grab FishHealthManager script
                FishHealthManager healthManager = hitObject.GetComponent<FishHealthManager>();

                // If script does not exist, try to get from parent
                if (healthManager == null)
                {
                    healthManager = hitObject.GetComponentInParent<FishHealthManager>();
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
