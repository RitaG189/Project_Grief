using TMPro;
using UnityEngine;

public abstract class Task : MonoBehaviour, IInteractable
{
    [SerializeField] private TasksSO taskSO;
    private TMP_Text interactionText;
    private bool canInteract = false;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
    }

    public void Interact()
    {
        if (!TaskManager.Instance.TryExecuteTask(taskSO))
            return;
        
        ExecuteTask();
    }

    public void ToggleVisibility(bool value)
    {
        if(interactionText != null)
        {
            if (!TaskManager.Instance.TryExecuteTask(taskSO))
                canInteract = false;
            else 
                canInteract = true;

            interactionText.enabled = value;
            interactionText.text = taskSO.taskName; 

            if(canInteract)
            {
                interactionText.alpha = 1;              
            }
            else if(!canInteract)
            {
                interactionText.alpha = .5f; 
            }

        }
    }

    protected abstract void ExecuteTask();
}
