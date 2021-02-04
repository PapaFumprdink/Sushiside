using UnityEngine;

[DisallowMultipleComponent]
public class Item : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] new private Rigidbody2D rigidbody;
    [SerializeField] private Vector2 heldOffset;

    public Vector2 Direction { get; set; }
    public Transform ParentTransform { get; set; }

    private void Start()
    {
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (!rigidbody) rigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    private void Update()
    {
        if (ParentTransform)
            transform.position = (Vector2)ParentTransform.position + heldOffset;
    }

    public Item Pickup (Transform parent)
    {
        ParentTransform = parent;

        rigidbody.angularVelocity = 0f;
        rigidbody.velocity = Vector2.zero;
        rigidbody.simulated = false;

        return this;
    }

    public Item Drop (Transform parent) => Drop(parent, Vector2.zero);
    public Item Drop (Transform parent, Vector2 throwForce)
    {
        ParentTransform = null;

        var parentRigidbody = parent.GetComponent<Rigidbody2D>();
        if (parentRigidbody)
        {
            rigidbody.simulated = true;
            rigidbody.velocity = parentRigidbody.velocity + throwForce / rigidbody.mass;
        }

        return this;
    }
}
