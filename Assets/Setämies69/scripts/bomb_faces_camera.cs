using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_faces_camera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Find the main camera at the start
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Make the plane always face the camera
            Vector3 lookDirection = mainCamera.transform.position - transform.position;
            lookDirection.y = 0; // Optional: keep the plane from tilting up/down
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}
