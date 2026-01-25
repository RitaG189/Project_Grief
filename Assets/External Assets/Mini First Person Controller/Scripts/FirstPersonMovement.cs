using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Walking")]
    public float speed = 5f;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9f;
    public KeyCode runningKey = KeyCode.LeftShift;

    [Header("Movement Smoothing")]
    public float acceleration = 20f;
    public float deceleration = 25f;

    [Header("Gravity")]
    public float gravity = -20f;
    public float groundSnapForce = -2f;

    private CharacterController controller;
    private Vector3 velocity;          // Y (gravidade)
    private Vector3 currentMove;       // XZ suavizado

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    private bool canMove = true;
    public bool IsSitted { get; private set; }

    [SerializeField] private PlayerAnimationController animationController;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (IsSitted && HasMovementInput())
        {
            Stand();
        }

        ApplyMovement();
    }

    void ApplyMovement()
    {
        if (!canMove)
        {
            currentMove = Vector3.zero;
            velocity.y = 0;
            controller.Move(Vector3.zero);
            animationController.SetMovementSpeed(0);
            return;
        }

        // ───── Running
        IsRunning = canRun && Input.GetKey(runningKey);

        float targetSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // ───── Input (RAW)
        Vector2 rawInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        rawInput = Vector2.ClampMagnitude(rawInput, 1f);

        // ───── Target movement
        Vector3 targetMove = Vector3.zero;
        if (rawInput != Vector2.zero)
        {
            Vector3 dir =
                transform.right * rawInput.x +
                transform.forward * rawInput.y;

            targetMove = dir.normalized * targetSpeed;
        }

        // ───── Accel / Decel
        float accel = rawInput != Vector2.zero ? acceleration : deceleration;
        currentMove = Vector3.MoveTowards(
            currentMove,
            targetMove,
            accel * Time.deltaTime
        );

        controller.Move(currentMove * Time.deltaTime);

        // ───── Gravity + ground snap
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = groundSnapForce;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(Vector3.up * velocity.y * Time.deltaTime);

        // ───── Animation speed
        float animSpeed = currentMove.magnitude;
        animationController.SetMovementSpeed(animSpeed);
    }

    bool HasMovementInput()
    {
        return Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f ||
               Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f;
    }

    // ───── External control
    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
        currentMove = Vector3.zero;
        velocity = Vector3.zero;
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
        EnableMovement();
        animationController.StandUp();
    }
}
