using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] String interactionName;
    private TMP_Text interactionText;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
    }

    public void Interact()
    {
        TaskManager.Instance.Sleep();
    }

    public void ToggleVisibility(bool value)
    {
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = interactionName;
        }
    }
}