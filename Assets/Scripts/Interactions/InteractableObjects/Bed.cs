using TMPro;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionName;
    private TMP_Text interactionText;
    //private Outline outline;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        
        //outline = GetComponent<Outline>();

        //outline.enabled = true;
        //outline.OutlineWidth = 0f;
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

        //outline.OutlineWidth = value ? 3f : 0f;
    }
}