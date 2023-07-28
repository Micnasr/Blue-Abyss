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

    [Header("Damage Distance Calculations")]
    public float maxDistanceForScaling = 30f;
    public float minDistanceForScaling = 8f;
    public float minDamagePercentage = 0.1f;

    [Header("Effect Prefabs")]
    public GameObject hitOnEffect;
    public GameObject bloodEffect;
    public GameObject waterSplash;

    [Header("DamageUI Data")]
    public GameObject damageUIPrefab;
    public Transform damageUISpawn;

    [Header("FishPointsUI Data")]
    public Transform fishPointsUISpawn;
    public Color32 orangeFishColor;

    private void Start()
    {
        //Subscribing to Event
        PlayerShoot.shootInput += Shoot;
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    int fishPoints = 0;

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

            // Process All Hits
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
                      
                    /* DetermineColorOfHit(hitInfo, i); YOU NEED TO SET TEXTURES AS READ/WRITE TO USE THIS */

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
                HandleDamage(hitObject, hitInfo, i);
            }

            // Spawn Fish Points UI
            if (fishPoints != 0)
                SpawnFishUI(fishPoints);

            fishPoints = 0;
            timeSinceLastShot = 0;
        }
    }

    public void ShowDamageUI(float damageValue)
    {
        // Instantiate the damageUI prefab at the spawn point
        GameObject damageUIInstance = Instantiate(damageUIPrefab, damageUISpawn.position, damageUISpawn.rotation);
        damageUIInstance.transform.SetParent(damageUISpawn);

        // Set the text on the child TextMeshPro component
        TMPro.TextMeshProUGUI textMeshPro = damageUIInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = "-" + damageValue.ToString();

            float interpolationFactor = Mathf.Clamp01(damageValue / gunData.damage);
            byte gValue = (byte)Mathf.Lerp(0f, 255f, interpolationFactor);

            // Color32 - Color would not work in this case - this took too long to realize :cry:
            textMeshPro.color = new Color32(255, (byte)(255 - gValue), 0, 255);
        }

        StartCoroutine(DestroyAfterAnimation(1f, damageUIInstance));
    }

    private IEnumerator DestroyAfterAnimation(float duration, GameObject objToDestroy)
    {
        yield return new WaitForSeconds(duration);
        Destroy(objToDestroy);
    }

    private void PlayColorEffect(RaycastHit[] hitInfo, int i, Color color)
    {
        GameObject newEffect = Instantiate(hitOnEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
        ParticleSystem particleSystem = newEffect.GetComponent<ParticleSystem>();
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = particleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(color);
        particleSystem.Play();
    }

    private void HandleDamage(GameObject hitObject, RaycastHit[] hitInfo, int i)
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
            // Calculate the Damage Based on Distance
            float distance = Vector3.Distance(transform.position, hitInfo[i].point);

            float damagePercentage;
            if (distance < minDistanceForScaling)
            {
                damagePercentage = 1f;
            }
            else if (distance >= minDistanceForScaling && distance <= maxDistanceForScaling)
            {
                // Scale the damage based on the distance
                damagePercentage = Mathf.Lerp(1f, minDamagePercentage, (distance - minDistanceForScaling) / (maxDistanceForScaling - minDistanceForScaling));
            }
            else
            {
                // Minimum damage beyond maxDistanceForScaling
                damagePercentage = minDamagePercentage;
            }

            // Calculate the actual damage
            float actualDamage = gunData.damage * damagePercentage;
            actualDamage = Mathf.Round(actualDamage * 10f) / 10f;

            // Apply Damage
            fishHealthManager.TakeDamage(actualDamage);

            if (fishHealthManager.currentHealth <= 0)
                fishPoints += fishHealthManager.deathPoints;

            Instantiate(bloodEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
            ShowDamageUI(actualDamage);
        }
        else if (hitInfo[i].transform.CompareTag("WaterCollider"))
        {
            // We Hit Water Surface
            FindObjectOfType<AudioManager>().Play("BulletSplash");
            Instantiate(waterSplash, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    // HitEffect will be of color of material hit :) ~MN
    private void DetermineColorOfHit(RaycastHit[] hitInfo, int i)
    {
        MeshRenderer meshRenderer = hitInfo[i].transform.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            Texture2D texture = meshRenderer.material.mainTexture as Texture2D;
            if (texture != null)
            {
                Vector2 pixelUV = hitInfo[i].textureCoord;
                pixelUV.x *= texture.width;
                pixelUV.y *= texture.height;
                Color color = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                PlayColorEffect(hitInfo, i, color);
            }
            else
            {
                Material mat = meshRenderer.material;

                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.color;
                    PlayColorEffect(hitInfo, i, color);
                }
                else
                {
                    Instantiate(hitOnEffect, hitInfo[i].point, Quaternion.LookRotation(hitInfo[i].normal));
                }
            }
        }
    }

    private void SpawnFishUI(int points)
    {
        GameObject pointsInstance = Instantiate(damageUIPrefab, fishPointsUISpawn.position, fishPointsUISpawn.rotation);
        pointsInstance.transform.SetParent(fishPointsUISpawn);

        TMPro.TextMeshProUGUI textMeshPro = pointsInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = "+" + points.ToString();
            textMeshPro.color = orangeFishColor;
            textMeshPro.fontSize = 40;
        }


        StartCoroutine(DestroyAfterAnimation(1f, pointsInstance));

    }
}