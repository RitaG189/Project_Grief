using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float lookDistance = 1.2f;
    [SerializeField] private float sphereRadius = 0.06f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Camera playerCamera;

    [Header("Stability")]
    [SerializeField] private float loseFocusDelay = 0.08f;

    private readonly HashSet<IInteractable> interactablesInRange = new();
    private IInteractable currentInteractable;

    private float loseFocusTimer;

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

        bool hasHit = Physics.SphereCast(
            ray,
            sphereRadius,
            out RaycastHit hit,
            lookDistance,
            interactableLayer,
            QueryTriggerInteraction.Ignore
        );


        if (hasHit)
        {
            lookedInteractable = hit.collider.GetComponentInParent<IInteractable>();

            if (lookedInteractable != null &&
                !interactablesInRange.Contains(lookedInteractable))
            {
                lookedInteractable = null;
            }
        }

        // ───── estabilidade / anti-flicker ─────
        if (lookedInteractable == currentInteractable)
        {
            loseFocusTimer = 0f;
            return;
        }

        loseFocusTimer += Time.deltaTime;

        if (loseFocusTimer < loseFocusDelay)
            return;

        if (currentInteractable != null)
            currentInteractable.ToggleVisibility(false);

        currentInteractable = lookedInteractable;

        if (currentInteractable != null)
            currentInteractable.ToggleVisibility(true);

        loseFocusTimer = 0f;
    }

    // ─────────────────────────────────────
    // Trigger range
    // ─────────────────────────────────────
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

        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;

        Gizmos.DrawRay(origin, direction * lookDistance);

        Gizmos.DrawWireSphere(
            origin + direction * lookDistance,
            sphereRadius
        );
    }
}
