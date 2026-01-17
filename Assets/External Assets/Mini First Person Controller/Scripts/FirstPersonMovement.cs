using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    private bool canMove = true;
    public bool IsSitted { get; private set; }
    [SerializeField] private PlayerAnimationController animationController;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (IsSitted && HasMovementInput())
        {
            Stand();
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rigidbody.linearVelocity = new Vector3(0, rigidbody.linearVelocity.y, 0);
            return;
        }

        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.linearVelocity.y, targetVelocity.y);

        float currentSpeed = new Vector2(
            rigidbody.linearVelocity.x,
            rigidbody.linearVelocity.z
        ).magnitude;

        animationController.SetMovementSpeed(currentSpeed);
    }

    bool HasMovementInput()
    {
        return Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f ||
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f;
    }

    public void EnableMovement() => canMove = true;

    public void DisableMovement()
    {
        canMove = false;
        rigidbody.linearVelocity = Vector3.zero;
    }

    public void ToggleSitting(bool value)
    {
        IsSitted = value;
    }

    public void Sit()
    {
        IsSitted = true;
        DisableMovement();
        animationController.SitDown();
    }

    public void Stand()
    {
        IsSitted = false;
        animationController.StandUp();
    }

}