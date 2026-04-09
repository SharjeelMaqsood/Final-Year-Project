using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Controller playerController;

    [Header("Animation Smoothing")]
    [SerializeField] private float accelerationRate = 5f;
    [SerializeField] private float decelerationRate = 8f;

    [Header("Animation Speed Mapping")]
    [SerializeField] private float animationSpeedMultiplier = 2f;

    private float currentAnimationSpeed;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (playerController == null)
            playerController = GetComponent<Controller>();

        currentAnimationSpeed = 0f;
    }

    private void Update()
    {
        if (animator != null)
            if (playerController == null) return;

        // Set isGrounded parameter
        animator.SetBool("isGrounded", playerController.isGrounded);


        {
            if (playerController == null) return;

            // Get values from controller
            float realSpeed = playerController.CurrentSpeed;
            Vector2 moveInput = playerController.MoveInput;
            bool isShiftPressed = playerController.IsShiftPressed;

            // If not moving, target speed is 0
            if (moveInput.magnitude < 0.1f)
            {
                realSpeed = 0f;
            }

            // Map real speed to animation speed
            float targetAnimationSpeed = realSpeed * animationSpeedMultiplier;

            // Smooth the animation speed
            if (targetAnimationSpeed > currentAnimationSpeed)
            {
                currentAnimationSpeed += accelerationRate * Time.deltaTime;
                if (currentAnimationSpeed > targetAnimationSpeed)
                    currentAnimationSpeed = targetAnimationSpeed;
            }
            else if (targetAnimationSpeed < currentAnimationSpeed)
            {
                currentAnimationSpeed -= decelerationRate * Time.deltaTime;
                if (currentAnimationSpeed < targetAnimationSpeed)
                    currentAnimationSpeed = targetAnimationSpeed;
            }

            // Send to animator
            animator.SetFloat("moveSpeed", currentAnimationSpeed);

            // Running state
            bool isRunning = isShiftPressed && moveInput.magnitude > 0.1f;
            animator.SetBool("isRunning", isRunning);
        }
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            animator.SetTrigger("Jump");
        }
    }
    public void OnAttack(InputValue value)
    {
        // Do nothing here
    }
    public void OnDodge(InputValue value)
    {
        if (value.isPressed && animator != null)
        {
            animator.SetTrigger("Dodge");
        }
    }
}