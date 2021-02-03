using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerControls))]
public class PlayerItemManager : MonoBehaviour
{
    [SerializeField] private PlayerControls controls;
    [SerializeField] private float pickupRange = 12f;
    [SerializeField] private float throwForce = 160f;

    public Item CurrentItem { get; private set; }

    private void Start()
    {
        if (!controls) controls = GetComponent<PlayerControls>();
    }

    private void Update()
    {
        if (controls.GetButtonPhase(InputButton.Throw) == InputPhase.Down)
        {
            if (CurrentItem)
            {
                CurrentItem.Drop(transform, controls.LookDirection * throwForce);
            }
            else
            {
                foreach (var query in Physics2D.OverlapCircleAll(transform.position, pickupRange))
                {
                    var item = query.GetComponent<Item>();
                    if (item)
                    {
                        CurrentItem = item.Pickup(transform);
                        return;
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
