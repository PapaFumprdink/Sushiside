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
    [SerializeField] private float downGravity = 4f;
    [SerializeField] private float upGravity = 2f;

    [Space]
    [SerializeField] private PlayerControls controls;
    [SerializeField] new private Rigidbody2D rigidbody;

    public Vector2 MovementDirection { get; private set; }
    public bool IsGrouned { get; private set; }

    private void Start()
    {
        if (!controls) controls = GetComponent<PlayerControls>();
        if (!rigidbody) rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckInput();
        MovePlayer();
        UpdateGravity();
    }

    private void CheckInput()
    {
        MovementDirection = new Vector2(controls.GetButtonValue(InputButton.Horizontal), controls.GetButtonValue(InputButton.Vertical));

        if (controls.GetButtonPhase(InputButton.Jump) == InputPhase.Down)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (IsGrouned && !canFly)
        {
            rigidbody.velocity = new Vector2(MovementDirection.x * leapPower, jumpPower);
        }
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
            rigidbody.gravityScale = (rigidbody.velocity.y < 0 || !controls.GetButtonState(InputButton.Jump)) ? downGravity : upGravity;
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
