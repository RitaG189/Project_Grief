using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float lookDistance = 1.2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Camera playerCamera;

    private readonly HashSet<IInteractable> interactablesInRange = new();
    private IInteractable currentInteractable;

    void Update()
    {
        DetectLookInteractable();

        if (Input.GetMouseButtonDown(0) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void DetectLookInteractable()
    {
        IInteractable lookedInteractable = null;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, lookDistance, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable) &&
                interactablesInRange.Contains(interactable))
            {
                lookedInteractable = interactable;
            }
        }

        // Se mudou o foco
        if (currentInteractable != lookedInteractable)
        {
            if (currentInteractable != null)
                currentInteractable.ToggleVisibility(false);

            currentInteractable = lookedInteractable;

            if (currentInteractable != null)
                currentInteractable.ToggleVisibility(true);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactablesInRange.Add(interactable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactablesInRange.Remove(interactable);

            if (currentInteractable == interactable)
            {
                currentInteractable.ToggleVisibility(false);
                currentInteractable = null;
            }
        }
    }

    // ─────────────────────────────────────
    // Gizmos
    // ─────────────────────────────────────
    void OnDrawGizmos()
    {
        if (!playerCamera) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * lookDistance
        );
    }
}
