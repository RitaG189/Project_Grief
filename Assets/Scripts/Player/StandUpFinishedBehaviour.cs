using UnityEngine;

public class StandUpFinishedBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.SendMessage(
            "OnStandUpAnimationFinished",
            SendMessageOptions.DontRequireReceiver
        );
    }
}
