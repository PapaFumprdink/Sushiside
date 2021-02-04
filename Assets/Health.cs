using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public event System.Action<GameObject, float, Vector2, Vector2> OnDamage;
    public event System.Action<GameObject, float, Vector2, Vector2> OnDeath;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [Space]
    [SerializeField] private SpriteRenderer outlineRenderer;
    [SerializeField] private SpriteRenderer iceRenderer;
    [SerializeField][Range(0f, 1f)] private float iceOpacity;
    [SerializeField] private float maxFreezeTime;

    [Space]
    [SerializeField] private Rigidbody2D deadBodyPrefab;
    [SerializeField] private float deadBodyForce;

    private float freezeTime;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsFrozen => freezeTime > 0;

    public void Damage(GameObject damager, float damage, Vector2 point, Vector2 direction)
    {
        currentHealth -= damage;

        OnDamage?.Invoke(damager, damage, point, direction);

        if (currentHealth < 0)
        {
            Kill(damager, damage, point, direction);
        }
    }

    public void Kill (GameObject killer, float damage, Vector2 point, Vector2 direction)
    {
        var deadBody = Instantiate(deadBodyPrefab, transform.position, transform.rotation);
        deadBody.velocity = direction * damage * deadBodyForce;

        OnDeath?.Invoke(killer, damage, point, direction);

        Destroy(gameObject);
    }

    private void Update()
    {
        freezeTime -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        ResetGraphics();
        UpdateGraphics();
    }

    private void ResetGraphics()
    {
        outlineRenderer.color = Color.clear;
        iceRenderer.color = Color.clear;
    }

    private void UpdateGraphics()
    {
        var frozenPercent = Mathf.Clamp01(freezeTime / maxFreezeTime);
        outlineRenderer.color = IsFrozen ? Color.white : Color.clear;
        iceRenderer.color = new Color(1, 1, 1, frozenPercent * iceOpacity);
    }

    public void Freeze (float duration)
    {
        freezeTime = duration;
    }
}
