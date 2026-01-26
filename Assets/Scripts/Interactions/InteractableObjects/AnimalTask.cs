using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalTask : Task
{
    public bool IsOnBox {get; set;} = false;
    private MemoryBoxItem item;

    protected override void Awake()
    {
        if (!Application.isPlaying) return;
        
        base.Awake();

        taskSO.taskDone = false;
        item = GetComponent<MemoryBoxItem>();
    }

    protected override void ExecuteTask()
    {
        if(item.itemData.level != LevelsManager.Instance.level)
        {
            return;
        }

        if(IsOnBox == false && TaskManager.Instance.TryExecuteAnimalTask(taskSO))
        {
            NeedsManager.Instance.ApplyTaskCostAndRewards(taskSO);
            PlayerHandManager.Instance.SetItemOnHand(gameObject);
            taskSO.taskDone = true;
            ToggleVisibility(false);
        }        
    }

    public override void ToggleVisibility(bool value)
    {
        if (item.itemData.level != LevelsManager.Instance.level || !TaskManager.Instance.TryExecuteAnimalTask(taskSO))
            canInteract = false;
        else
            canInteract = true;
        
        base.ToggleVisibility(value);
    }

}
