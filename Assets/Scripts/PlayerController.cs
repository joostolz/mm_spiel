using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Make isGrounded visible in the inspector for debugging
    [SerializeField] private bool isGrounded = true;
    // Distance for the ground check raycast
    [SerializeField] private float groundCheckDistance = 0.05f;
    // Time between allowed jumps
    [SerializeField] private float jumpRepeatTime = 1f;

    // Movement parameters
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    private float lastJumpTime;
    // A reference to the Animator component
    private Animator animator;
    // A reference to the CharacterController component
    private CharacterController controller;
    // Private variable for vertical velocity (gravity and jump)
    private float verticalVelocity;

    void Start()
    {
        // Get references to the Animator and CharacterController components
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check based on raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        // Draw the raycast to visualize it in the editor
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundCheckDistance), Color.red);

        // Get the horizontal and vertical input from the player as a Vector2
        Vector2 input = InputSystem.actions["Player/Move"].ReadValue<Vector2>();

        // Create a 3D vector for horizontal movement direction
        Vector3 moveDir = new Vector3(input.x, 0, input.y);

        // Determine if the player is moving
        bool isMoving = moveDir != Vector3.zero;

        // Set the "isRunning" param in the animator
        animator.SetBool("isRunning", isMoving);

        // -- Movement & Rotation --
        if (isMoving)
        {
            // Calcualte the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);

            // Smoothly rotate the character towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Fix to ensure the character is grounded
        if (controller.isGrounded)
        {
            // Apply a downward force to keep the player grounded
            verticalVelocity = -2f;
        }

        // Jump Logic
        if (InputSystem.actions["Player/Jump"].WasPressedThisFrame() && isGrounded && Time.time > lastJumpTime + jumpRepeatTime)
        {
            // Calculate the jump force using the fomula v = sqrt(-2gh)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
            // Trigger the jump animation
            animator.SetTrigger("Jump");
            // Set the last jump time to the current time
            lastJumpTime = Time.time;
        }

        // Apply gravity to vertical velocity
        verticalVelocity += gravity * Time.deltaTime;
        // Combine horizontal and vertical movement
        Vector3 finalMovement = moveDir * movementSpeed + new Vector3(0f, verticalVelocity, 0f);
        // Move the character controller
        controller.Move(finalMovement * Time.deltaTime);
    }
}
