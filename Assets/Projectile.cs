using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    const float SKIN_WIDTH = 0.1f;

    [SerializeField] private float damage;
    [SerializeField] private float muzzleVelocity;
    [SerializeField] private LayerMask hitscanMask;
    [SerializeField] private float lifetime;

    new Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * muzzleVelocity;

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        var speed = rigidbody.velocity.magnitude * Time.deltaTime;
        var hit = Physics2D.Raycast(rigidbody.position, rigidbody.velocity, speed + SKIN_WIDTH, hitscanMask);

        if (hit)
        {
            Destroy(gameObject);
        }
    }
}
