using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pilviRotation : MonoBehaviour
{
    public float minY = 0f; // Minimum Y value
    public float maxY = 90f; // Maximum Y value
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    private float targetY;

    void Update()
    {
        // Rotate the object
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Get the current rotation angle around Y
        float currentY = transform.eulerAngles.y;

        // Check if the current Y is close to the target, reverse direction if necessary
        if (currentY >= maxY || currentY <= minY)
        {
            rotationSpeed = -rotationSpeed;
        }
    }
}
