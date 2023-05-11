using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public bool cursorLocked = false;
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    public float maxAngle = 60;
    public Transform cams;
    private Quaternion camCenter;

    void Start()
    {
        camCenter = cams.localRotation;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        UpdateCursorLock();
    }
    void UpdateCursorLock()
    {
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = true;
            }
        }
    }
}
