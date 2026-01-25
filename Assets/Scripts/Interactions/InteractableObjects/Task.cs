using System.Collections;
using TMPro;
using UnityEngine;

public abstract class Task : MonoBehaviour, IInteractable
{
    [SerializeField] protected TasksSO taskSO;
    [SerializeField] float cooldown = 4f;
    bool onCooldown = false;
    protected TMP_Text interactionText;
    protected bool canInteract = false;
    public bool TaskEnabled {get; private set;} = true;
    private Outline outline;
    //[SerializeField] NeedsPreviewController needsPreviewController;


    protected virtual void Awake()
    {
        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        outline = GetComponent<Outline>();
    }

    public void Interact()
    {
        if (!TaskEnabled || onCooldown) return;
        if (PlayerHandManager.Instance.IsHoldingItem) return;

        if (!TaskManager.Instance.TryExecuteTask(taskSO))
            return;

        ExecuteTask();
        StartCoroutine(CooldownRoutine());
    }

    public virtual void ToggleVisibility(bool value)
    {
        if(interactionText != null && taskSO.taskDone == false)
        {
            if(taskSO.category != TaskCategory.Animal)
            {
                if (!NeedsManager.Instance.CanPerformTask(taskSO))
                    canInteract = false;
                else
                    canInteract = true;
            }

            interactionText.enabled = value;
            interactionText.text = taskSO.taskName; 

            interactionText.alpha = canInteract ? 1f : 0.2f;
        }
        
        if(outline != null)
            outline.enabled = value;


        /*
        if (!value)
        {
            needsPreviewController.ClearPreview();
            return;
        }

        if (taskSO != null && canInteract)
            needsPreviewController.ShowTaskPreview(taskSO);
        else
            needsPreviewController.ClearPreview();
            */
    }

    public void EnableTask(bool value)
    {
        TaskEnabled = value;
    }

    IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        interactionText.alpha = 0.2f;

        yield return new WaitForSeconds(cooldown);

        onCooldown = false;
        
        interactionText.alpha = canInteract ? 1f : 0.2f;
    }

    protected abstract void ExecuteTask();
}
