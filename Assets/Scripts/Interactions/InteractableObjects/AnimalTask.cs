using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalTask : Task
{
    [SerializeField] PlayerAnimationController playerAnimations;
    
    protected override void ExecuteTask()
    {
        //playerAnimations.PickUpItem();
        PlayerHandManager.Instance.SetItemOnHand(gameObject);
        
    }
}
