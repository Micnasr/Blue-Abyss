using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCam : MonoBehaviour
{
    //Mouse Sensitivity
    public float sens;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public Slider sensSlider;

    private void Awake()
    {
        sens = PlayerPrefs.GetFloat("MouseSensitivity", sens);
        sensSlider.value = sens;
    }

    void Start()
    {
        //Lock Cursor & Visibility 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateSensitivity(float newSensitivity)
    {
        // Update Mouse Sensitivity
        sens = newSensitivity;
        PlayerPrefs.SetFloat("MouseSensitivity", sens);
        PlayerPrefs.Save();
    }


    // Update is called once per frame (apparently time.delta time for camera movement is very bad)
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sens;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
