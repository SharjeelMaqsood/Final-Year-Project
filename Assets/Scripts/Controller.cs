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
    [SerializeField] private LayerMask groundLayer = ~0;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraSwitcher cameraSwitcher;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    public bool isGrounded;
    private float currentSpeed;
    private float verticalVelocity;

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
        isGrounded = characterController.isGrounded;

        // Speed handling
        if (IsShiftPressed && moveInput.magnitude > 0.1f)
            currentSpeed = moveSpeed * shiftMultiplier;
        else
            currentSpeed = moveSpeed;

        // 🔥 SEPARATED MODES
        if (cameraSwitcher != null && cameraSwitcher.IsFirstPerson)
        {
            HandleFirstPerson();
        }
        else
        {
            HandleThirdPerson();
        }

        HandleGravity();
    }

    // ✅ FIRST PERSON (unchanged logic)
    private void HandleFirstPerson()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    // ✅ THIRD PERSON (fixed jitter)
    private void HandleThirdPerson()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;

        characterController.Move(move * currentSpeed * Time.deltaTime);

        // 🔥 ROTATION (fixed)
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 moveDirection = move;

            if (moveDirection.sqrMagnitude < 0.01f)
                return;

            // 🔥 Prevent backward jitter
            float forwardDot = Vector3.Dot(transform.forward, moveDirection.normalized);

            if (forwardDot < -0.5f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void HandleGravity()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 verticalMove = new Vector3(0, verticalVelocity, 0);
        characterController.Move(verticalMove * Time.deltaTime);
    }

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

    private void OnDrawGizmosSelected()
    {
        if (characterController != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                transform.position - Vector3.up * characterController.height / 2,
                groundCheckDistance
            );
        }
    }
}