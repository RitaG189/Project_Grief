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
        if(movement.IsSitted == false)
        {
            PlayerInteractionController.Instance.SitOnCouch(
                lookAtPoint
            );
            canvas.SetActive(false);  
        }
    }

    public void ToggleVisibility(bool value)
    {
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = name;
        }
    }
}
