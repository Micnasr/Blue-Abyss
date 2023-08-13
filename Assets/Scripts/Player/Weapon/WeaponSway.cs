using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [Header("Sway Settings")]
    [SerializeField] private float speed;

    public PlayerCam playerCam;

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * playerCam.sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * playerCam.sens;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}