using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalTask : Task
{
    [SerializeField] PlayerAnimationController playerAnimations;
    public bool IsOnBox {get; set;} = false;
    
    protected override void ExecuteTask()
    {
        ToggleVisibility(false);
        //playerAnimations.PickUpItem();
        if(IsOnBox == false)
        {
            NeedsManager.Instance.ApplyTaskCostAndRewards(taskSO);
            PlayerHandManager.Instance.SetItemOnHand(gameObject);
            taskSO.taskDone = true;
        }
        
        
    }

}
