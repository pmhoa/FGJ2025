using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    private Rigidbody rb;
    public float delayTime = 2f; // Time after which movement will stop
    public GameObject explosion;
    private bool isArmed = false;
    public Camera targetCamera; // Reference to the camera

    private Vector3 minePos;


    public MeshRenderer rend;
    public Material[] mats;

    public int matnum;


    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody found on this GameObject. Please add one for movement control.");
        }


        // Start the coroutine to stop movement after the delay
        StartCoroutine(ArmMineCoroutine());



    }

    void Update()
    {
        if (targetCamera != null)
        {
            // Make the object's forward direction face the camera
            Vector3 direction = targetCamera.transform.position - transform.position;
            direction.z = 0; // Lock the z-axis for 2D objects
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        if (!isArmed)
        {
            minePos = gameObject.transform.position;
        }

        if (isArmed)
        {
            transform.position = minePos;
        }
    }

    void matswap1()
    {
        rend.material = mats[1];
    }

    // Coroutine to stop movement after the specified delay
    private IEnumerator ArmMineCoroutine()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delayTime);

        isArmed = true;
        matswap1();
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && isArmed == true)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
