using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private FirstPersonMovement movement;
    [SerializeField] private PlayerCameraController cameraController;

    public void SitDown()
    {   
        cameraController.SetFollowMode(PlayerCameraController.CameraFollowMode.FollowHead);
        animator.SetBool("IsSitted", true);
    }

    public void StandUp()
    {
        animator.SetTrigger("StandUp");
        animator.SetBool("IsSitted", false);
    }

    public void SetMovementSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void Sleep()
    {
        animator.SetTrigger("Sleep");
    }

    public void OnStandUpAnimationFinished()
    {
        movement.EnableMovement();
        cameraController.SetFollowMode(PlayerCameraController.CameraFollowMode.Static);
    }

    public void EnterShower()
    {
        
    }
}
