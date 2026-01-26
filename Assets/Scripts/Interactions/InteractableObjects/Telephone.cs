using TMPro;
using UnityEngine;

public class Telephone : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject telephoneMenu;
    [SerializeField] protected TasksSO taskSO;
    protected TMP_Text interactionText;
    protected bool canInteract = false;
    public bool TaskEnabled {get; private set;} = true;
    private Outline outline;

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
        if(TaskEnabled && !PlayerHandManager.Instance.IsHoldingItem)
        {
            if (!NeedsManager.Instance.CanPerformTask(taskSO))
                return;
        
            NeedsManager.Instance.ApplyTaskCostAndRewards(taskSO);

            ToggleVisibility(false);
            ToggleTelephoneMenu();
        }
    }


    private void ToggleTelephoneMenu()
    {
        telephoneMenu.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Paused);
    }

    public void CloseTelephone()
    {
        telephoneMenu.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Gameplay);
        ToggleVisibility(false);
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

            interactionText.alpha = canInteract ? 1f : 0.2f;
        }

        outline.OutlineWidth = value ? 3f : 0f;
    }

    public void CallMom()
    {
        
    }

    public void CallFriend()
    {
        
    }
}
