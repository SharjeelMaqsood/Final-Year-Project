using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private Inventory playerInventory;

    private void Update()
    {
        // Check if E was pressed this frame
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnInteract();
        }
    }

    private void OnInteract()
    {
        Debug.Log("E Pressed");

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);

            ItemPickup pickup = hit.collider.GetComponentInParent<ItemPickup>();

            if (pickup != null)
            {
                Debug.Log("Item Pickup Found");

                pickup.Pickup(playerInventory);
            }
            else
            {
                Debug.Log("Hit object is NOT pickup");
            }
        }
        else
        {
            Debug.Log("Ray did NOT hit anything");
        }
    }

    private void OnDrawGizmos()
    {
        if (cameraTransform == null) return;

        Gizmos.color = Color.red;

        Vector3 origin = cameraTransform.position;
        Vector3 direction = cameraTransform.forward;

        Gizmos.DrawLine(origin, origin + direction * interactDistance);
    }
}