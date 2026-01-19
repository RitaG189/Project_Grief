using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInteractionController : MonoBehaviour
{
    public static PlayerInteractionController Instance;

    [SerializeField] FirstPersonMovement movement;
    [SerializeField] FirstPersonLook look;
    [SerializeField] PlayerAnimationController animations;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] PlayerCameraController cameraController; // Adiciona esta referência

    void Awake()
    {
        Instance = this;
    }

    public void SitOnCouch(Transform sitPoint, Transform lookAtPoint)
    {
        if (movement.IsSitted) return;
        StartCoroutine(SitRoutine(sitPoint, lookAtPoint));
    }

    //public void 

    IEnumerator GoTo(Transform sitPoint)
    {
        movement.DisableMovement();
        look.DisableLook();

        agent.enabled = true;
        agent.isStopped = false;
        agent.updateRotation = false;

        agent.SetDestination(sitPoint.position);

        yield return new WaitUntil(() =>
            !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            agent.velocity.sqrMagnitude < 0.01f
        );

        agent.isStopped = true;
        agent.enabled = false;
    }

    IEnumerator SitRoutine(Transform sitPoint, Transform lookAtPoint)
    {
        //StartCoroutine(GoTo(sitPoint));
        yield return GoTo(sitPoint);

        yield return RotateTowards(lookAtPoint.position);
        
        look.DisableLook();

        cameraController.SetFollowMode(PlayerCameraController.CameraFollowMode.FollowHead); 
        movement.Sit();
    }

    IEnumerator RotateTowards(Vector3 target)
    {
        look.ClearInput();              
        look.DisableLook();             
        look.EnableExternalRotation();  

        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        Quaternion targetCameraPitch = Quaternion.identity;

        float timer = 0f;
        float maxTime = 0.4f;

        while (Quaternion.Angle(transform.rotation, targetRot) > 1f && timer < maxTime)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                8f * Time.deltaTime
            );

            look.transform.localRotation = Quaternion.Slerp(
                look.transform.localRotation,
                targetCameraPitch,
                8f * Time.deltaTime
            );

            timer += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
        look.transform.localRotation = targetCameraPitch;

        look.LockCurrentRotation(); // 🔒 trava no fim
    }

}
