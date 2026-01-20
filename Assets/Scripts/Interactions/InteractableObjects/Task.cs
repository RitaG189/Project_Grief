using TMPro;
using UnityEngine;

public abstract class Task : MonoBehaviour, IInteractable
{
    [SerializeField] protected TasksSO taskSO;
    protected TMP_Text interactionText;
    private bool canInteract = false;
    public bool TaskEnabled {get; private set;} = true;

    void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
    }

    public void Interact()
    {
        if(TaskEnabled)
        {
            if (!TaskManager.Instance.TryExecuteTask(taskSO))
                return;
        
            ExecuteTask();
        }
    }

    public void ToggleVisibility(bool value)
    {
        if(interactionText != null && taskSO.taskDone == false)
        {
            if (!NeedsManager.Instance.CanPerformTask(taskSO))
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
                interactionText.alpha = .2f; 
            }

        }
    }

    public void EnableTask(bool value)
    {
        TaskEnabled = value;
    }

    protected abstract void ExecuteTask();
}
