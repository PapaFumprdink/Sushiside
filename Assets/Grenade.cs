using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Item))]
public class Grenade : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] new private Rigidbody2D rigidbody;

    [Space]
    [SerializeField] private float detonateDelay;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private ParticleSystem pinEffect;
    [SerializeField] private SpriteRenderer flashRenderer;
    [SerializeField] private float flashTime;
    [SerializeField] private float flashPercent;

    bool isPrimed = false;

    private void Start()
    {
        if (!item) item = GetComponent<Item>();
        if (!rigidbody) rigidbody = GetComponent<Rigidbody2D>();

        flashRenderer.enabled = false;
    }

    private void Update()
    {
        if (item.ParentTransform)
        {
            var controls = item.ParentTransform.GetComponent<PlayerControls>();
            if (!isPrimed && controls.GetButtonState(InputButton.Fire))
            {
                Prime();
            }
        }
    }

    public virtual void Prime()
    {
        isPrimed = true;
        pinEffect.Play();

        StartCoroutine(DetonateRoutine());
    }

    private IEnumerator DetonateRoutine()
    {
        var percent = 0f;
        while (percent < 1f)
        {
            flashRenderer.enabled = ((percent * detonateDelay) % flashTime) / flashTime > flashPercent;

            percent += Time.deltaTime / detonateDelay;
            yield return null;
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}