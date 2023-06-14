using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    #region Variables
    Vector3 startPosition;
    public float swaySensitivity = 2f;
    public float swayClamp = 20f;
    public float swaySmoothness = 20f;
    Vector3 nextPosition;
    Vector3 currentVelocity = Vector3.zero;
    #endregion
    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * swaySensitivity / 100 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * swaySensitivity / 100 * Time.deltaTime;

        mouseX = Mathf.Clamp(mouseX, -swayClamp, swayClamp);
        mouseY = Mathf.Clamp(mouseY, -swayClamp, swayClamp);

        nextPosition = new Vector3(mouseX, mouseY, 0);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, nextPosition + startPosition, ref currentVelocity, Time.deltaTime * swaySmoothness);
    }
}
