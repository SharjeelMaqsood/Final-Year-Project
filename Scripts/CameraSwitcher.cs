using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public bool IsFirstPerson => isFirstPerson;

    public Camera firstPersonCamera;
    public GameObject thirdPersonCamera;

    public MonoBehaviour mouseLookScript;

    private bool isFirstPerson = true;

    private void Start()
    {
        SetFirstPerson();
    }

    public void OnSwitchCamera(InputValue value)
    {
        if (value.isPressed)
            ToggleCamera();
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

        Vector3 rot = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0f, rot.y, 0f);

        isFirstPerson = false;
    }
}