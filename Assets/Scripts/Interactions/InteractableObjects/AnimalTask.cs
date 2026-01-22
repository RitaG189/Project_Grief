using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalTask : Task
{
    public bool IsOnBox {get; set;} = false;
    private MemoryBoxItem item;

    protected override void Awake()
    {
        base.Awake();

        taskSO.taskDone = false;
        item = GetComponent<MemoryBoxItem>();
    }

    protected override void ExecuteTask()
    {
        ToggleVisibility(false);

        if(item.itemData.level != LevelsManager.Instance.level)
        {
            return;
        }

        if(IsOnBox == false)
        {
            NeedsManager.Instance.ApplyTaskCostAndRewards(taskSO);
            PlayerHandManager.Instance.SetItemOnHand(gameObject);
            taskSO.taskDone = true;
        }        
    }

    public override void ToggleVisibility(bool value)
    {
        if (item.itemData.level != LevelsManager.Instance.level)
            canInteract = true;
        else
            canInteract = false;
        
        base.ToggleVisibility(value);
    }

}
