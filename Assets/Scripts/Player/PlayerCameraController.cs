using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] Transform headBone;
    [SerializeField] Transform cameraPivot;
    [SerializeField] GameObject camera;

    [SerializeField] float followSpeed = 8f;
    [SerializeField] FirstPersonLook look;

    CameraFollowMode mode = CameraFollowMode.Static;

    public enum CameraFollowMode
    {
        Static,      // FPS normal
        FollowHead   // sentar, dormir, cutscene
    }

    public void SetFollowMode(CameraFollowMode newMode)
    {
        mode = newMode;

        if (mode == CameraFollowMode.FollowHead)
        {
            camera.transform.SetParent(headBone);
            look.DisableLook();

        }
        else if(mode == CameraFollowMode.Static)
        {
            camera.transform.SetParent(cameraPivot);
            look.EnableLook();

        }
    }
}
