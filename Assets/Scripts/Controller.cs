using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float shiftMultiplier = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer = ~0; // Everything by default
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    public bool isGrounded;
    private float currentSpeed;
    private float verticalVelocity;

    // Public properties for animation
    public float CurrentSpeed => currentSpeed;
    public Vector2 MoveInput => moveInput;
    public bool IsShiftPressed => Keyboard.current.leftShiftKey.isPressed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component missing!");
        }
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        // Ground check using CharacterController's built-in isGrounded
        isGrounded = characterController.isGrounded;

        // Update speed based on shift key
        if (IsShiftPressed && moveInput.magnitude > 0.1f)
        {
            currentSpeed = moveSpeed * shiftMultiplier;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Handle movement
        HandleMovement();

        // Handle gravity and jumping
        HandleGravity();

        // Handle Rotation
        HandleRotation();
    }

    private void HandleMovement()
    {
        float x = moveInput.x;
        float z = moveInput.y;  
        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * currentSpeed * Time.deltaTime);
    }
    private void HandleGravity()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Small value to keep grounded
        }

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Apply vertical movement (jumping + gravity)
        Vector3 verticalMove = new Vector3(0, verticalVelocity, 0);
        characterController.Move(verticalMove * Time.deltaTime);
    }

    private void HandleRotation()
    {
        if (moveInput.magnitude > 0.1f)
        {
            Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
    // Called by PlayerInput
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Optional: Visualize ground check in editor
    private void OnDrawGizmosSelected()
    {
        if (characterController != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - Vector3.up * characterController.height / 2, groundCheckDistance);
        }
    }
    
}