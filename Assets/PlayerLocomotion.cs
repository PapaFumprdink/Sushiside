using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerControls))]
public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private float maxMovementSpeed = 160f;
    [SerializeField] private float movementAcceleration = 640f;
    [SerializeField] private float airAcceleration = 320f;

    [Space]
    [SerializeField] private float jumpPower = 160f;
    [SerializeField] private float leapPower = 160f;
    [SerializeField] private bool canFly = false;

    [Space]
    [SerializeField] private Transform groundedPoint;
    [SerializeField] private float groundedRadius = 2f;
    [SerializeField] private LayerMask groundedMask = ~0;

    [Space]
    [SerializeField] private float downGravity = 64f;
    [SerializeField] private float upGravity = 32f;

    [Space]
    [SerializeField] private PlayerControls controls;
    [SerializeField] new private Rigidbody2D rigidbody;

    private bool wantsToJump;

    public Vector2 MovementDirection { get; private set; }
    public bool IsGrouned { get; private set; }

    private void Start()
    {
        if (!controls) controls = GetComponent<PlayerControls>();
        if (!rigidbody) rigidbody = GetComponent<Rigidbody2D>();

        controls.OnInputButtonEvent += InputButtonEvent;
    }

    private void InputButtonEvent(InputButton button, InputPhase phase, float value)
    {
        switch (button)
        {
            case InputButton.Up:
                MovementDirection += Vector2.up;
                break;

            case InputButton.Down:
                MovementDirection += Vector2.down;
                break;

            case InputButton.Left:
                MovementDirection += Vector2.left;
                break;

            case InputButton.Right:
                MovementDirection += Vector2.right;
                break;

            case InputButton.Jump:
                if (phase == InputPhase.Down)
                {
                    Jump();
                    wantsToJump = true;
                }
                else if (phase == InputPhase.Up)
                {
                    wantsToJump = false;
                }
                break;
        }
    }

    private void Jump ()
    {
        if (IsGrouned && !canFly)
        {
            rigidbody.velocity = new Vector2(MovementDirection.x * leapPower, jumpPower);
        }
    }

    private void Update()
    {
        MovePlayer();
        UpdateGravity();
    }

    private void MovePlayer()
    {
        var localVelocity = rigidbody.velocity;
        var targetVelocity = MovementDirection * maxMovementSpeed;
        var acceleration = (IsGrouned ? movementAcceleration : airAcceleration) * Time.deltaTime;

        localVelocity.x = Mathf.MoveTowards(localVelocity.x, targetVelocity.x, acceleration);
        if (canFly) localVelocity.y = Mathf.MoveTowards(localVelocity.y, targetVelocity.y, acceleration);

        rigidbody.velocity = localVelocity;

        MovementDirection = Vector2.zero;
    }

    private void UpdateGravity ()
    {
        if (canFly)
            rigidbody.gravityScale = 0;
        else
            rigidbody.gravityScale = (rigidbody.velocity.y < 0 || !wantsToJump) ? downGravity : upGravity;
    }

    private void FixedUpdate()
    {
        IsGrouned = GetGrounded();
    }

    private bool GetGrounded()
    {
        var queries = new Collider2D[2];
        Physics2D.OverlapCircleNonAlloc(groundedPoint.position, groundedRadius, queries, groundedMask);

        foreach (var query in queries)
        {
            if (query)
            {
                if (query.gameObject != gameObject)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundedPoint)
        {
            var isGrounded = GetGrounded();

            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawSphere(groundedPoint.position, groundedRadius);
            Gizmos.color = Color.white;
        }
    }
}
