using TMPro;
using UnityEngine;

public abstract class Task : MonoBehaviour, IInteractable
{
    [SerializeField] protected TasksSO taskSO;
    protected TMP_Text interactionText;
    protected bool canInteract = false;
    public bool TaskEnabled {get; private set;} = true;
    [SerializeField] NeedsPreviewController needsPreviewController;


    protected virtual void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
    }

    public void Interact()
    {
        if(TaskEnabled && !PlayerHandManager.Instance.IsHoldingItem)
        {
            if (!TaskManager.Instance.TryExecuteTask(taskSO))
                return;
        
            ExecuteTask();
        }
    }

    public virtual void ToggleVisibility(bool value)
    {
        if(interactionText != null && taskSO.taskDone == false)
        {
            if (!NeedsManager.Instance.CanPerformTask(taskSO))
                canInteract = false;
            else
                canInteract = true;

            interactionText.enabled = value;
            interactionText.text = taskSO.taskName; 

            interactionText.alpha = canInteract ? 1f : 0.2f;
        }

        if (!value)
        {
            needsPreviewController.ClearPreview();
            return;
        }

        if (taskSO != null && canInteract)
            needsPreviewController.ShowTaskPreview(taskSO);
        else
            needsPreviewController.ClearPreview();
    }

    public void EnableTask(bool value)
    {
        TaskEnabled = value;
    }

    protected abstract void ExecuteTask();
}
