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

    IEnumerator SitRoutine(Transform sitPoint, Transform lookAtPoint)
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

        yield return RotateTowards(lookAtPoint.position);
        
        look.DisableLook();

        cameraController.SetFollowMode(PlayerCameraController.CameraFollowMode.FollowHead); 
        movement.Sit();
    }

    IEnumerator RotateTowards(Vector3 target)
    {
        look.EnableExternalRotation();
        
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        Quaternion targetCameraPitch = Quaternion.identity; // Pitch 0
        
        float timer = 0f;
        float maxTime = .5f;

        while (Quaternion.Angle(transform.rotation, targetRot) > 1f && timer < maxTime)
        {
            // Roda o personagem
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                5f * Time.deltaTime
            );
            
            // Roda a câmara para pitch 0
            look.transform.localRotation = Quaternion.Slerp(
                look.transform.localRotation,
                targetCameraPitch,
                5f * Time.deltaTime
            );
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        // Força a rotação final
        transform.rotation = targetRot;
        look.transform.localRotation = targetCameraPitch;
        
        // Atualiza os valores locked
        look.ResetCameraPitch();
    }
}
