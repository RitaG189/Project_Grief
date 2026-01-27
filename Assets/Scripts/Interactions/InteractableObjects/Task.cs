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
        if (!Application.isPlaying) return;

        interactionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TMP_Text>();
        
        outline = GetComponent<Outline>();

        outline.enabled = true;
        outline.OutlineWidth = 0f;
    }

    public void Interact()
    {
        if (!TaskEnabled || onCooldown) return;
        if (PlayerHandManager.Instance.IsHoldingItem) return;

        if (!TaskManager.Instance.TryExecuteTask(taskSO))
            return;

        ExecuteTask();
        StartCoroutine(CooldownRoutine());

        NeedsUIController.Instance.ClearPreview();
    }


    public virtual void ToggleVisibility(bool value)
    {
        if (interactionText != null && !taskSO.taskDone)
        {
            if (taskSO.category != TaskCategory.Animal)
                canInteract = NeedsManager.Instance.CanPerformTask(taskSO);
            else
                canInteract = true;

            interactionText.enabled = value;
            interactionText.text = taskSO.taskName;
            interactionText.alpha = canInteract ? 1f : 0.2f;
        }

        outline.OutlineWidth = value ? 3f : 0f;

        if(value = true) 
            NeedsUIController.Instance.ShowTaskPreview(taskSO);

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
