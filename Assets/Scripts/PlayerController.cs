using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int Hitpoints = 3;
    bool speedboost = false;
    Vector3 startpoint;

    // Make isGrounded visible in the inspector for debugging
    [SerializeField] private bool isGrounded = true;
    // Distance for the ground check raycast
    [SerializeField] private float groundCheckDistance = 0.05f;
    // Time between allowed jumps
    [SerializeField] private float jumpRepeatTime = 0.2f;
    [SerializeField] private float jumpRepeatTime = 1f;
    
        // ✅ NEU: public damit PowerUp_Speed darauf zugreifen kann
    [HideInInspector] public float movementSpeed = 5f;

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;


    private float lastJumpTime;
    // A reference to the Animator component
    private Animator animator;
    // A reference to the CharacterController component
    private CharacterController controller;
    // Private variable for vertical velocity (gravity and jump)
    private float verticalVelocity;
    
    
    //Camera parameters
    [SerializeField] GameObject mainCam; 
    [SerializeField] Transform cameraFollowTarget;
    float xRotation;
    float yRotation;
    [SerializeField] float empfindlich = 0.2f;

    void Start()
    {
        // Get references to the Animator and CharacterController components
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        startpoint = transform.position;
    }

    void Update()
    {
        // Ground check based on raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        //Debug.Log(isGrounded);
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

        float sprintput = InputSystem.actions["Player/Sprint"].ReadValue<float>();
        Debug.Log(sprintput);
        float targetRotation = 0;
        float speed = 0;

        // -- Movement & Rotation --
        if (isMoving)
        {   
            speed = movementSpeed;

            if (sprintput == 1)
            {
                if (speedboost)
                {
                    speed = speed * 4;
                }
            }
            // Calcualte the target rotation based on the movement direction
            targetRotation = Quaternion.LookRotation(moveDir).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            
            //mach Quaternion draus für Rotationsberechnung
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            
            // Smoothly rotate the character towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
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
        Vector3 finalMovement = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward * speed + new Vector3(0f, verticalVelocity, 0f);
        // Move the character controller
        controller.Move(finalMovement * Time.deltaTime);

        CameraRotation();

        if (Hitpoints <= 0)
        {
            transform.position = startpoint;
            Hitpoints = 1;
        }

    }

    void CameraRotation()
    {

        Vector2 inputcamera = InputSystem.actions["Player/Look"].ReadValue<Vector2>();
        xRotation += inputcamera.y * empfindlich * 0.5f;
        yRotation += inputcamera.x * empfindlich;
        xRotation = Mathf.Clamp(xRotation,-10,50);
        Quaternion camrotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = camrotation;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "dead")
        {
            Hitpoints = 0;
        }
        if (hit.gameObject.tag == "hp")
        {
            Hitpoints++;
        }
        if(hit.gameObject.tag == "boost")
        {
            speedboost = true;
        }
    }
}
