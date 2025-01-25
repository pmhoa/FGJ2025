using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuplaFoam : MonoBehaviour
{
    private Rigidbody rb;
    public float delayTime = 1f; // Time after which movement will stop
    public GameObject explosion;

    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody found on this GameObject. Please add one for movement control.");
        }

        // Start the coroutine to stop movement after the delay
        StartCoroutine(StopMovementCoroutine());
    }

    // Coroutine to stop movement after the specified delay
    private IEnumerator StopMovementCoroutine()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delayTime);

        // Stop the Rigidbody's movement by setting its velocity to zero
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; // Stops rotation if desired
            //Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            
        }
    }
}
