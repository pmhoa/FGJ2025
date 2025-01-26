using UnityEngine;

using System.Collections;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player
    public int health = 3;
    public int gameEndDelay = 3;
    [SerializeField]
    Animator animator;
    [SerializeField]
    private GameObject deadFace;

    private bool flipCharacter;
    private bool dead;
    private CharacterController characterController;
    private Vector3 velocity;
    //private bool isGrounded;
    private Coroutine deathCorotine;

    private Vector3 impactForce = Vector3.zero; // Force to be applied over time
    public float forceStrength = 10f; // Magnitude of the force

    //public GameObject endGameObject;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        flipCharacter = false;
        dead = false;
    }

    void Update()
    {
        // Check if the player is grounded
        //isGrounded = characterController.isGrounded;

        // Get input for movement (WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (dead == false)
        {
            // Calculate the movement vector
            Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;

            if (move.x < 0 && flipCharacter == false)
            {
                animator.gameObject.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                flipCharacter = true;
            }
            else if (move.x > 0 && flipCharacter == true)
            {
                animator.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                flipCharacter = false;
            }

            // Apply movement directly in the input direction (WASD controls)
            characterController.Move(move * moveSpeed * Time.deltaTime);

            // If there is movement, snap the player to face the cursor
            FaceTowardsCursor();




            if (impactForce.magnitude > 0.1f)
            {
                characterController.Move(impactForce * Time.deltaTime);
                // Gradually reduce the force (dampening effect)
                impactForce = Vector3.Lerp(impactForce, Vector3.zero, 5f * Time.deltaTime);
            }








            //// Handle gravity
            //if (isGrounded && velocity.y < 0)
            //{
            //    velocity.y = -2f; // Keep the player grounded
            //}

            // Jump
            //if (isGrounded && Input.GetButtonDown("Jump"))
            //{
            //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //}

            // Apply gravity to the vertical velocity
            //velocity.y += gravity * Time.deltaTime;

            // Apply vertical movement (gravity and jumping)
            characterController.Move(velocity * Time.deltaTime);

            if (health <= 0)
            {
                dead = true;
                deathCorotine = StartCoroutine(DeathRoutine());
            }
            animator.SetFloat("Speed", Mathf.Abs(move.x));
            animator.SetFloat("SpeedVertical", Mathf.Abs(move.z));
        }
        if(dead == true)
        {
            animator.SetBool("Dead", dead);
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
        deadFace.SetActive(true);    
        yield return new WaitForSeconds(gameEndDelay);
        SceneManager.LoadScene("Smale");
        //endGameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyProjectile")
        {
            health--;

            Vector3 forceDirection = (transform.position - other.transform.position).normalized;

            // Apply force
            impactForce = forceDirection * forceStrength;
            impactForce.y = 0;

            //CharacterController cC = GetComponent<CharacterController>();
            //Rigidbody rb = GetComponent<Rigidbody>();
            //Vector3 forceDir = other.transform.forward * forceStrength;
            //forceDir.y = 0;
            //cC.Move(forceDir * 100f * Time.deltaTime);
            Destroy(other.gameObject);
            //cC.AddForce(forceDir.normalized * 100f, ForceMode.Impulse);

        }
    }
}
