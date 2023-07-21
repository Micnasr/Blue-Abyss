using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;

    public Transform muzzle;

    public Camera playerCam;

    public Animator gunController;

    float timeSinceLastShot;

    public Color shieldEffectColor;

    public LayerMask obstacleLayers;

    [Header("Effect Prefabs")]
    public GameObject hitOnEffect;
    public GameObject bloodEffect;
    public GameObject waterSplash;

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

            // Play Gun Shot Animation
            gunController.SetTrigger("GunShot");

            // List to store already hit objects
            List<GameObject> hitObjects = new List<GameObject>();

            // Sort the hitInfo array based on hit distance (this helps with rendering effects with superimposed meshes)
            System.Array.Sort(hitInfo, (a, b) => a.distance.CompareTo(b.distance));

            for (int i = 0; i < hitInfo.Length; i++)
            {
                GameObject hitObject = hitInfo[i].transform.gameObject;

                if (hitObject.CompareTag("shield"))
                {
                    PlayColorEffect(hitInfo, i, shieldEffectColor);
                    break;
                }

                // If we hit an obstacle, we do not care about what we hit behind
                if ((obstacleLayers.value & (1 << hitObject.layer)) != 0)
                {
                    Instantiate(hitOnEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
                    break;
                }

                // Check if the hit object has a parent
                if (hitInfo[i].transform.parent != null)
                    hitObject = hitInfo[i].transform.parent.gameObject;

                // Check if the same object has been hit before at the same hit point
                if (hitObjects.Contains(hitObject))
                    continue;

                // Add the hit object to the list
                hitObjects.Add(hitObject);

                // Handle Damage to Fish & HitEffect
                DamageAndHit(hitObject, hitInfo, i);
            }

            timeSinceLastShot = 0;
        }
    }

    private void PlayColorEffect(RaycastHit[] hitInfo, int i, Color color)
    {
        GameObject newEffect = Instantiate(hitOnEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
        ParticleSystem particleSystem = newEffect.GetComponent<ParticleSystem>();
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = particleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(color);
        particleSystem.Play();
    }

    private void DamageAndHit(GameObject hitObject, RaycastHit[] hitInfo, int i)
    {
        // Grab FishHealthManager script
        FishHealthManager fishHealthManager = hitObject.GetComponent<FishHealthManager>();

        // If script does not exist, try to get from parent
        if (fishHealthManager == null)
        {
            fishHealthManager = hitObject.GetComponentInParent<FishHealthManager>();
        }

        // If hit target has a Fish Health Manager (Its a damagable specie)
        if (fishHealthManager != null)
        {
            // Call TakeDamage function
            fishHealthManager.TakeDamage(gunData.damage);
            Instantiate(bloodEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
        }
        else if (hitInfo[i].transform.CompareTag("WaterCollider"))
        {
            // We Hit Water Surface
           Instantiate(waterSplash, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }
}
