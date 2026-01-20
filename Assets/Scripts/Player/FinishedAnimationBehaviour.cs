using UnityEngine;

public class FinishedAnimationBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.SendMessage(
            "OnAnimationFinished",
            SendMessageOptions.DontRequireReceiver
        );
    }
}
