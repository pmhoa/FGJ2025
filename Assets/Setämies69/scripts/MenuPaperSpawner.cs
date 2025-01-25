using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPaperSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;  // Reference to the object prefab
    public float minInterval = 1f;    // Minimum time interval for spawning (in seconds)
    public float maxInterval = 5f;    // Maximum time interval for spawning (in seconds)

    private void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnObjects());
    }

    private System.Collections.IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Wait for a random time between minInterval and maxInterval
            float randomInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomInterval);

            // Spawn the object at the current position
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }
    }
}
