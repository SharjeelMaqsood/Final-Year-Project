using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;   // Player (Y rotation)
    public Transform cameraPivot;  // Empty object (X rotation)

    private PlayerInputActions controls;

    float xRotation = 0f;

    void Awake()
    {
        controls = new PlayerInputActions();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 mouseInput = controls.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        // 🔹 Vertical rotation (camera only)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -40f, 80f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 🔹 Horizontal rotation (player)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}