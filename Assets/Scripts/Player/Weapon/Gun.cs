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

            // List to store already hit objects
            List<GameObject> hitObjects = new List<GameObject>();

            // Sort the hitInfo array based on hit distance (this helps with rendering effects with superimposed meshes)
            System.Array.Sort(hitInfo, (a, b) => a.distance.CompareTo(b.distance));

            for (int i = 0; i < hitInfo.Length; i++)
            {
                GameObject hitObject = hitInfo[i].transform.gameObject;

                // Check if the hit object has a parent
                if (hitInfo[i].transform.parent != null)
                    hitObject = hitInfo[i].transform.parent.gameObject;

                // Check if the same object has been hit before at the same hit point
                if (hitObjects.Contains(hitObject))
                    continue;

                // Create Hit on Effect
                Instantiate(hitOnEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));

                // Add the hit object to the list
                hitObjects.Add(hitObject);

                // Grab FishHealthManager script
                FishHealthManager fishHealthManager = hitObject.GetComponent<FishHealthManager>();

                // If script does not exist, try to get from parent
                if (fishHealthManager == null)
                {
                    fishHealthManager = hitObject.GetComponentInParent<FishHealthManager>();
                }

                if (fishHealthManager != null)
                {
                    // Call TakeDamage function
                    fishHealthManager.TakeDamage(gunData.damage);
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
