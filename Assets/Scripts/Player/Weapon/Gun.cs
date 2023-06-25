using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;

    float timeSinceLastShot;

    private void Start()
    {
        //Subscribing to Event
        PlayerShoot.shootInput += Shoot;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    public void Shoot()
    {
        if (CanShoot())
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
            {
                Debug.Log(hitInfo.transform.name);
            }

            timeSinceLastShot = 0;
            OnGunShot();
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private static void OnGunShot()
    {
        
    }
}
