using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : MonoBehaviour
{
    public float lifetime = 5f;  // Time before the object destroys itself
    public float forwardSpeed = 5f;  // Speed at which the object flies forward
    public float fallSpeed = 1f;  // Speed of the falling effect (gravity-like)

    private Vector3 initialVelocity;  // Initial velocity of the object

    void Start()
    {
        // Destroy the object after the specified lifetime
        Destroy(gameObject, lifetime);

        // Set initial forward velocity
        initialVelocity = transform.forward * forwardSpeed;
    }

    void Update()
    {
        // Apply forward motion to the object
        transform.position += initialVelocity * Time.deltaTime;

        // Apply slight falling effect
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }
}
