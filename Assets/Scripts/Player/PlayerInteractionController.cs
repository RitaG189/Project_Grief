using System.Collections;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public static PlayerInteractionController Instance;

    [SerializeField] FirstPersonMovement movement;
    [SerializeField] FirstPersonLook look;
    [SerializeField] PlayerCameraController cameraController;

    void Awake()
    {
        if (!Application.isPlaying) return;
        
        Instance = this;
    }

    public void SitOnCouch(Transform lookAtPoint)
    {
        if (movement.IsSitted) return;
        StartCoroutine(SitRoutine(lookAtPoint));
    }

    IEnumerator SitRoutine(Transform lookAtPoint)
    {
        movement.DisableMovement();

        yield return RotateTowards(lookAtPoint.position);

        look.transform.localRotation = Quaternion.identity;

        cameraController.SetFollowMode(
            PlayerCameraController.CameraFollowMode.FollowHead
        );

        movement.Sit();
    }

    IEnumerator RotateTowards(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        float duration = 0.35f;
        float t = 0f;
        Quaternion startRot = transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
    }
}
