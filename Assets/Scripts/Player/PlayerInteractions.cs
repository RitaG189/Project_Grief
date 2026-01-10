using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 1.2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Camera playerCamera;
    private IInteractable currentInteractable;

    void Update()
    {
        DetectInteractable();

        if (Input.GetMouseButtonDown(0) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void DetectInteractable()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                currentInteractable.ToggleVisibility(true);
                return;
            }
        }

        if(currentInteractable  != null)
            currentInteractable.ToggleVisibility(false);

        currentInteractable = null;
    }

    void OnDrawGizmos()
    {
        if (!playerCamera) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * interactDistance
        );
    }
}
