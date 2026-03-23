using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;
    public GameObject thirdPersonCamera;

    public MonoBehaviour mouseLookScript;

    private PlayerInputActions controls;
    private bool isFirstPerson = true;

    private void Start()
    {
            SetFirstPerson();
    }
    private void Awake()
    {
        controls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.SwitchCamera.performed += ctx => ToggleCamera();
    }

    private void OnDisable()
    {
        controls.Player.SwitchCamera.performed -= ctx => ToggleCamera();
        controls.Disable();
    }

    void ToggleCamera()
    {
        if (isFirstPerson)
            SetThirdPerson();
        else
            SetFirstPerson();
    }

    void SetFirstPerson()
    {
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.SetActive(false);

        mouseLookScript.enabled = true;

        isFirstPerson = true;
    }

    void SetThirdPerson()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.SetActive(true);

        mouseLookScript.enabled = false;

        // reset player tilt
        Vector3 rot = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0f, rot.y, 0f);

        isFirstPerson = false;
    }
}