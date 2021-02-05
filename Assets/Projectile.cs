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

    [Space]
    [SerializeField] private float freezeTime;

    [Space]
    [SerializeField] private GameObject hitEffect;

    new Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * muzzleVelocity;

        transform.right = rigidbody.velocity.normalized;

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        var speed = rigidbody.velocity.magnitude * Time.deltaTime;
        var hit = Physics2D.Raycast(rigidbody.position, rigidbody.velocity, speed + SKIN_WIDTH, hitscanMask);

        if (hit)
        {
            var health = hit.transform.GetComponent<Health>();
            if (health && freezeTime > 0f)
            {
                health.Freeze(freezeTime);
            }

            Instantiate(hitEffect, hit.point, Quaternion.Euler(0, 0, Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg));
            Destroy(gameObject);
        }
    }
}
