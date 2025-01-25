using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    private Rigidbody rb;
    public float delayTime = 2f; // Time after which movement will stop
    public GameObject explosion;
    private bool isArmed = false;

    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody found on this GameObject. Please add one for movement control.");
        }

        // Start the coroutine to stop movement after the delay
        StartCoroutine(ArmMineCoroutine());
    }

    // Coroutine to stop movement after the specified delay
    private IEnumerator ArmMineCoroutine()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delayTime);

        isArmed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
