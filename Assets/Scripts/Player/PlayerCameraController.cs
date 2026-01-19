using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] Transform headBone;
    [SerializeField] Transform headCameraPivot;
    [SerializeField] Transform cameraPivot;
    [SerializeField] GameObject camera;

    [Header("Offsets")]
    [SerializeField] Vector3 headOffset = new Vector3(0f, 0.07f, 0.06f);

    [SerializeField] FirstPersonLook look;

    CameraFollowMode mode = CameraFollowMode.Static;

    public enum CameraFollowMode
    {
        Static,
        FollowHead
    }

    void LateUpdate()
    {
        if (mode == CameraFollowMode.FollowHead)
        {
            headCameraPivot.position = headBone.position;
            headCameraPivot.rotation = headBone.rotation;
        }
    }

    public void SetFollowMode(CameraFollowMode newMode)
    {
        mode = newMode;

        if (mode == CameraFollowMode.FollowHead)
        {
            look.DisableLook();
            look.LockCurrentRotation();

            headCameraPivot.SetParent(null);
            camera.transform.SetParent(headCameraPivot, false);

            camera.transform.localPosition = headOffset;
            camera.transform.localRotation = Quaternion.identity;
        }
        else
        {
            camera.transform.SetParent(cameraPivot, false);
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localRotation = Quaternion.identity;

            look.EnableLook();
        }
    }
}
