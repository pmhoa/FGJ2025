using UnityEngine;
using System.Collections;

public class FollowCursorAtDistanceWithSpawnAndShoot : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public GameObject objectToSpawn; // The projectile you want to spawn
    public float distanceFromPlayer = 2f; // Distance away from the player (in units)
    public float projectileSpeed = 10f; // Speed at which the projectile moves
    public int chosenWeapon = 1;
    public GameObject foamToSpawn;

    private Coroutine shootingCoroutine;



    void Update()
    {
        // Get the position of the mouse in world space (on the ground plane)
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(player.position).z; // Set the z to be the same as the player's position
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Get the direction from the player to the cursor
        Vector3 directionToCursor = worldMousePosition - player.position;

        // Set the y component to 0 to avoid tilting up or down
        directionToCursor.y = 0;

        // Normalize the direction vector to get a unit vector
        directionToCursor.Normalize();

        // Calculate the position 2 units away from the player in the direction of the cursor
        Vector3 targetPosition = player.position + directionToCursor * distanceFromPlayer;

        // Set the object's position to the target position
        transform.position = targetPosition;

        // Check if the player clicked the mouse (left button)
        if (Input.GetMouseButtonDown(0)) // Left click (0)
        {
            if(chosenWeapon == 1)
            {
                SpawnAndShootProjectile(targetPosition, directionToCursor);
            }

            else if (chosenWeapon == 2)
            {
                //SpawnExplosiveArea(targetPosition, directionToCursor);
                if(shootingCoroutine == null)
                {
                    shootingCoroutine = StartCoroutine(ShootExplosiveArea());
                }
                
            }
            
        }
        if (Input.GetMouseButtonUp(0) && chosenWeapon == 2)
        {
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine); // Stop shooting when the mouse button is released
                shootingCoroutine = null;
            }
        }
    }
    private IEnumerator ShootExplosiveArea()
    {
        while (true)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(player.position).z; // Set the z to be the same as the player's position
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 directionToCursor = worldMousePosition - player.position;

            // Set the y component to 0 to avoid tilting up or down
            directionToCursor.y = 0;

            // Normalize the direction vector to get a unit vector
            directionToCursor.Normalize();

            // Calculate the position 2 units away from the player in the direction of the cursor
            Vector3 targetPosition = player.position + directionToCursor * distanceFromPlayer;

            // Set the object's position to the target position
            transform.position = targetPosition;

            SpawnExplosiveArea(targetPosition, directionToCursor);
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds before shooting again
        }
    }
    // Method to spawn and shoot a projectile towards the cursor
    void SpawnAndShootProjectile(Vector3 spawnPosition, Vector3 shootDirection)
    {
        if (objectToSpawn != null)
        {
            // Instantiate the projectile at the target position with no rotation
            GameObject projectile = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Add a Rigidbody to the projectile if it doesn't have one (you can remove this if your projectile already has a Rigidbody)
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = projectile.AddComponent<Rigidbody>(); // Add Rigidbody if it doesn't exist
            }

            // Set the velocity of the projectile to shoot towards the cursor
            rb.velocity = shootDirection * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("No object assigned to spawn!");
        }
    }

    void SpawnExplosiveArea(Vector3 spawnPosition, Vector3 shootDirection)
    {
        if (foamToSpawn != null)
        {
            // Instantiate the projectile at the target position with no rotation
            GameObject projectile = Instantiate(foamToSpawn, spawnPosition, Quaternion.identity);

            // Add a Rigidbody to the projectile if it doesn't have one (you can remove this if your projectile already has a Rigidbody)
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = projectile.AddComponent<Rigidbody>(); // Add Rigidbody if it doesn't exist
            }

            // Introduce a random variation to the X-axis
            float randomX = Random.Range(-0.5f, 0.5f); // Random value between -1 and 1 (you can adjust this range)
            shootDirection.x += randomX;  // Add the random variation to the X-axis

            // Set the velocity of the projectile to shoot towards the direction with variation
            rb.velocity = shootDirection.normalized * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("No object assigned to spawn!");
        }
    }
}
