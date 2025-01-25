using UnityEngine;

using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player
    public float gravity = -9.8f; // Gravity for the player
    public float jumpHeight = 2f; // Jump height
    public int health = 3;
    public int gameEndDelay = 3;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private Coroutine deathCorotine;

    public GameObject endGameObject;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = characterController.isGrounded;

        // Get input for movement (WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the movement vector
        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;

        // Apply movement directly in the input direction (WASD controls)
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // If there is movement, snap the player to face the cursor
        FaceTowardsCursor();

        // Handle gravity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keep the player grounded
        }

        // Jump
        //if (isGrounded && Input.GetButtonDown("Jump"))
        //{
        //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}

        // Apply gravity to the vertical velocity
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement (gravity and jumping)
        characterController.Move(velocity * Time.deltaTime);

        if (health <= 0)
        {
            deathCorotine = StartCoroutine(DeathRoutine());
        }
    }

    // Function to make the player face the cursor
    void FaceTowardsCursor()
    {
        // Get the position of the mouse in world space (on the ground plane)
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Set the z to be the same as the player's position
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Get the direction from the player to the cursor
        Vector3 directionToCursor = worldMousePosition - transform.position;

        // Set the y component to 0 to avoid tilting the player up or down
        directionToCursor.y = 0;

        // If the player is moving, rotate to face the cursor
        if (directionToCursor.sqrMagnitude > 0.01f) // To avoid small floating-point errors
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCursor);
            transform.rotation = targetRotation; // Snap to the direction of the cursor
        }

        
    }

    private IEnumerator DeathRoutine()
    {
            yield return new WaitForSeconds(gameEndDelay);
            endGameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyProjectile")
        {
            health--;
        }
    }
}
