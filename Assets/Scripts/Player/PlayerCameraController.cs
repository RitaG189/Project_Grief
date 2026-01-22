using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] Transform headBone;
    [SerializeField] Transform cameraPivot;
    private GameObject mainCamera;

    [SerializeField] float followSpeed = 8f;
    [SerializeField] FirstPersonLook look;

    CameraFollowMode mode = CameraFollowMode.Static;

    public enum CameraFollowMode
    {
        Static,      // FPS normal
        FollowHead   // sentar, dormir, cutscene
    }

    void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void SetFollowMode(CameraFollowMode newMode)
    {
        mode = newMode;

        if (mode == CameraFollowMode.FollowHead)
        {
            mainCamera.transform.SetParent(headBone);
            look.DisableLook();

        }
        else if(mode == CameraFollowMode.Static)
        {
            mainCamera.transform.SetParent(cameraPivot);
            look.EnableLook();

        }
    }
}
