using UnityEngine;

public class CameraAlignedQuad : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // rotação EXACTA da câmara
        transform.rotation = cam.transform.rotation;
    }
}
