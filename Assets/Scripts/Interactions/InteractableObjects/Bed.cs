using TMPro;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionName;
    private TMP_Text interactionText;
    private Outline outline;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        outline = GetComponent<Outline>();
    }

    public void Interact()
    {
        if(!PlayerHandManager.Instance.IsHoldingItem)
        {
            ToggleVisibility(false);
            TaskManager.Instance.Sleep();  
        }
    }

    public void ToggleVisibility(bool value)
    {
        interactionText.alpha = 1;    
        
        if(interactionText != null)
        {
            interactionText.enabled = value;
            interactionText.text = interactionName;
        }

        outline.enabled = value;
    }
}