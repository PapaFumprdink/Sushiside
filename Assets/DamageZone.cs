using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DamageZone : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private Bounds bounds;

    private void FixedUpdate()
    {
        var queryList = Physics2D.OverlapBoxAll(transform.position + bounds.center, bounds.size, 0f);
        foreach (var query in queryList)
        {
            var health = query.transform.GetComponent<Health>();
            if (health)
            {
                health.Damage(gameObject, damage, query.transform.position, Vector2.up);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawCube(transform.position + bounds.center, bounds.size);
        Gizmos.color = Color.white;
    }
}
