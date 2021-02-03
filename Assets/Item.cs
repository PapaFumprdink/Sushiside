using UnityEngine;

[DisallowMultipleComponent]
public class Item : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Vector2 Direction
    {
        get => spriteRenderer.transform.right;
        set
        {
            spriteRenderer.flipY = value.x < 0;
            spriteRenderer.transform.right = value;
        }
    }
    public Rigidbody Parent { get; set; }

    public Item Pickup (Rigidbody parent)
    {
        return this;
    }

    public Item Drop (Rigidbody parent)
    {
        return this;
    }
}
