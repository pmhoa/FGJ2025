using UnityEngine;
using System.Collections;

public class FollowCursorAtDistanceWithSpawnAndShoot : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public GameObject basicKuplaPrefab; // The projectile you want to spawn
    public float distanceFromPlayer = 2f; // Distance away from the player (in units)
    public float projectileSpeed = 10f; // Speed at which the projectile moves
    public int chosenWeapon = 1;
    public GameObject foamToSpawn;
    public float foamShootRate = 0.1f;

    private Coroutine shootingCoroutine;



    public GameObject minePrefab;    // The projectile prefab to be shot
    public Transform shootingPoint;    // The point where the object will be spawned
    public float launchAngle = 45f;
    public float maxRange = 50f;



    void Update()
    {




        if (Input.GetKeyDown(KeyCode.Alpha1))  // Key 1 pressed
        {
            chosenWeapon = 1;
            Debug.Log("Value changed to: " + chosenWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))  // Key 2 pressed
        {
            chosenWeapon = 2;
            Debug.Log("Value changed to: " + chosenWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))  // Key 3 pressed
        {
            chosenWeapon = 3;
            Debug.Log("Value changed to: " + chosenWeapon);
        }
        //if (Input.GetKeyDown(KeyCode.Alpha4))  // Key 4 pressed
        //{
        //    chosenWeapon = 4;
        //    Debug.Log("Value changed to: " + chosenWeapon);
        //}












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

            else if (chosenWeapon == 2) // 2 is foam gun
            {
                //SpawnExplosiveArea(targetPosition, directionToCursor);
                if(shootingCoroutine == null)
                {
                    shootingCoroutine = StartCoroutine(ShootExplosiveArea());
                }
                
            }
            else if (chosenWeapon == 3)
            {
                ShootMine();
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


    void ShootMine()
    {
        // Perform a raycast from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Determine the target position
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;

            // Check if the target is within range
            float distanceToTarget = Vector3.Distance(shootingPoint.position, targetPoint);
            if (distanceToTarget > maxRange)
            {
                // Adjust the target to the maximum range in the direction of the cursor
                Vector3 direction = (targetPoint - shootingPoint.position).normalized;
                targetPoint = shootingPoint.position + direction * maxRange;
            }
        }
        else
        {
            // If the raycast doesn't hit anything, shoot to the max range in the cursor's direction
            Vector3 direction = ray.direction.normalized;
            targetPoint = shootingPoint.position + direction * maxRange;
        }

        // Spawn the projectile at the shooting point
        GameObject projectile = Instantiate(minePrefab, shootingPoint.position, Quaternion.identity);

        // Calculate the velocity required to launch the projectile in an arc
        Vector3 velocity = CalculateLaunchVelocity(shootingPoint.position, targetPoint, launchAngle);

        // Add the velocity to the projectile's Rigidbody
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = velocity;
        }
    }

    Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float angle)
    {
        // Convert the angle to radians
        float radianAngle = angle * Mathf.Deg2Rad;

        // Calculate the horizontal and vertical distances
        Vector3 direction = target - start;
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        float verticalDistance = direction.y;

        // Calculate the velocity needed to reach the target
        float gravity = Physics.gravity.y;
        float speedSquared = (horizontalDistance * horizontalDistance * gravity) /
                             (2 * (verticalDistance - Mathf.Tan(radianAngle) * horizontalDistance) * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle));
        if (speedSquared <= 0)
        {
            Debug.LogError("No solution found for the given angle and target position.");
            return Vector3.zero;
        }
        float speed = Mathf.Sqrt(Mathf.Abs(speedSquared));

        // Calculate the velocity components
        Vector3 velocity = new Vector3(direction.x, 0, direction.z).normalized;
        velocity *= speed * Mathf.Cos(radianAngle);
        velocity.y = speed * Mathf.Sin(radianAngle);

        return velocity;
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
            yield return new WaitForSeconds(foamShootRate); // Wait for 0.1 seconds before shooting again
        }
    }
    // Method to spawn and shoot a projectile towards the cursor
    void SpawnAndShootProjectile(Vector3 spawnPosition, Vector3 shootDirection)
    {
        if (basicKuplaPrefab != null)
        {
            // Instantiate the projectile at the target position with no rotation
            GameObject projectile = Instantiate(basicKuplaPrefab, spawnPosition, Quaternion.identity);

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
            float randomValue = Random.Range(0.3f, 1f); //This gives the random velocity
            // Set the velocity of the projectile to shoot towards the direction with variation
            rb.velocity = shootDirection.normalized * projectileSpeed * randomValue;
        }
        else
        {
            Debug.LogWarning("No object assigned to spawn!");
        }
    }
}
