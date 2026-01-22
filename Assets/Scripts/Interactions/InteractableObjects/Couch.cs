using System;
using TMPro;
using UnityEngine;

public class Couch : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject canvas;
    private TMP_Text interactionText;
    [SerializeField] string name;
    [SerializeField] FirstPersonMovement movement;
    [SerializeField] Transform sitPoint;
    [SerializeField] Transform lookAtPoint;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        interactionText.text = name;
    }

    void Update()
    {
        if(movement.IsSitted == true)
        {
            interactionText.enabled = false;
        }
    }

    public void Interact()
    {
        if(movement.IsSitted == false && !PlayerHandManager.Instance.IsHoldingItem)
        {
            PlayerInteractionController.Instance.SitOnCouch(
                lookAtPoint
            );
            ToggleVisibility(false);

        }
    }

    public void ToggleVisibility(bool value)
    {
        interactionText.alpha = 1;    
        
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = name;
        }
    }
}
