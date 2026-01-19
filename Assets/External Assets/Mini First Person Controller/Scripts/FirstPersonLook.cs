using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    public bool CanLook { get; private set; } = true;

    Vector2 velocity;
    Vector2 frameVelocity;
    
    bool lockRotation = false;
    float lockedYaw = 0f;
    float lockedPitch = 0f;

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!CanLook)
            return;

        Vector2 mouseDelta = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }

    void LateUpdate()
    {
        if (!CanLook)
            return;

        if (lockRotation)
        {
            character.localRotation = Quaternion.Euler(0, lockedYaw, 0);
            transform.localRotation = Quaternion.Euler(lockedPitch, 0, 0);
        }
    }

    public void EnableLook()
    {
        CanLook = true;
        lockRotation = false;
        
        velocity.x = character.localEulerAngles.y;
        velocity.y = -transform.localEulerAngles.x;
        
        frameVelocity = Vector2.zero;
    }

    public void DisableLook()
    {
        CanLook = false;
        lockRotation = true;
        
        lockedYaw = character.localEulerAngles.y;
        lockedPitch = transform.localEulerAngles.x;
    }
    
    // NOVO: Permite rotação externa sem mouse input
    public void EnableExternalRotation()
    {
        lockRotation = false;
    }
    public void DisableExternalRotation()
    {
        lockRotation = false;
    }
    
    // NOVO: Trava na rotação atual
    public void LockCurrentRotation()
    {
        lockRotation = true;
        lockedYaw = character.localEulerAngles.y;
        lockedPitch = transform.localEulerAngles.x;
    }

    public void ResetCameraPitch()
    {
        // Reset do pitch da câmara (eixo X)
        transform.localRotation = Quaternion.identity; // Ou Quaternion.Euler(0, 0, 0)
        lockedPitch = 0f;
        velocity.y = 0f;
    }

    public void ClearInput()
    {
        velocity = Vector2.zero;
        frameVelocity = Vector2.zero;
    }

}