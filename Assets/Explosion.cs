using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Explosion : MonoBehaviour
{
    [SerializeField] private float size = 32f;
    [SerializeField] private float maxDamage = 100f;
    [SerializeField] private AnimationCurve damageFalloff;
    [SerializeField] private bool onStart = true;

    private void Start()
    {
        if (onStart)
            Explode();
    }

    public void Explode()
    {
        var queryList = Physics2D.OverlapCircleAll(transform.position, size);
        foreach (var query in queryList)
        {
            var health = query.GetComponent<Health>();
            if (health)
            {
                var vector = (health.transform.position - transform.position);
                var direction = vector.normalized;
                var distance = vector.magnitude / size;
                var damage = damageFalloff.Evaluate(distance) * maxDamage;
                health.Damage(gameObject, damage, query.transform.position, direction);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, size);
        Gizmos.color = Color.white;
    }
}
