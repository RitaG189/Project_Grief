using TMPro;
using UnityEngine;

public class Alarm : MonoBehaviour, IInteractable
{
    [SerializeField] int hoursToSleep = 8;
    [SerializeField] string interactionName;
    private TMP_Text interactionText;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
    }

    public void Interact()
    {
        if(!PlayerHandManager.Instance.IsHoldingItem)
        {
            ToggleVisibility(false);
            TaskManager.Instance.Sleep(hoursToSleep);            
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
    }
}
