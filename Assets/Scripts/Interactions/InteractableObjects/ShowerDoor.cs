using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShowerDoor : MonoBehaviour, IInteractable
{
    private TMP_Text interactionText;
    private string interactionName = "Open";

    private Animator animator;
    private bool doorValue = false;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
    }

    public void ToggleVisibility(bool value)
    {
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = interactionName;
        }
    }

    public void Interact()
    {
        doorValue = !doorValue;

        if(doorValue == true)
            interactionText.text = "Close";
        else   
            interactionText.text = "Open";

        animator.SetBool("IsOpen", doorValue);
    }
}
