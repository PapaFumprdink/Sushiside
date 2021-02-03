using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Item))]
public class GunProfile : MonoBehaviour
{
    [SerializeField] PlayerControls controls;
    [SerializeField] new Rigidbody2D rigidbody;

    [Space]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float firerate;
    [SerializeField] bool singlefire;
    [SerializeField] float spray;
    [SerializeField] int projectileCount;
    [SerializeField] int burstCount;
    [SerializeField] float recoilAmount;
    [SerializeField] float recoilDrag;
    [SerializeField] float knockback;

    [Space]
    [SerializeField] int magazineSize;
    [SerializeField] float reloadTime;

    [Space]
    [SerializeField] Item item;
    [SerializeField] Transform root;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform muzzle;
    [SerializeField] Cinemachine.CinemachineImpulseSource shakeSource;

    private float nextFireTime;
    private float recoilVelocity;

    public Vector2 FacingDirection { get; private set; }

    void Start ()
    {
        if (!item) item = GetComponentInChildren<Item>();
    }

    private void Update()
    {
        UpdateRotation();
        CheckInput();
    }

    private void UpdateRotation()
    {
        FacingDirection = controls.LookDirection;

        spriteRenderer.flipX = FacingDirection.x < 0f;

        root.right = FacingDirection;
        root.localScale = new Vector3(1, FacingDirection.x < 0 ? -1 : 1, 1);

        var baseRotation = Mathf.Atan2(FacingDirection.y, FacingDirection.x) * Mathf.Rad2Deg;
        root.rotation = Quaternion.Euler(0, 0, baseRotation + recoilVelocity);

        recoilVelocity -= recoilVelocity * recoilDrag * Time.deltaTime;
    }

    private void CheckInput()
    {
        if (item.ParentTransform)
        {
            var controls = item.ParentTransform.GetComponent<PlayerControls>();

            if (controls)
            {
                var inputPhase = controls.GetButtonPhase(InputButton.Fire);

                if (singlefire && inputPhase == InputPhase.Down)
                {
                    if (burstCount > 1)
                    {
                        StartCoroutine(Burst());
                    }
                    else
                    {
                        Shoot();
                    }
                }
                else if (!singlefire && inputPhase == InputPhase.Held)
                {
                    Shoot();
                }
            }
        }
    }

    private void InputButtonEvent(PlayerControls controls, InputButton button, InputPhase phase, float value)
    {
        if (enabled && gameObject.activeSelf)
        {
            if (button == InputButton.Fire && Time.time > nextFireTime)
            {
                if (singlefire && phase == InputPhase.Down)
                {
                    if (burstCount > 1)
                    {
                        StartCoroutine(Burst());
                    }
                    else
                    {
                        Shoot();
                    }
                }
                else if (!singlefire && phase == InputPhase.Held)
                {
                    Shoot();
                }
            }
        }
    }

    private IEnumerator Burst()
    {
        nextFireTime = Time.time + (60f / firerate) * (burstCount + 1);

        for (int i = 0; i < burstCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(60f / firerate);
        }
    }

    private void Shoot()
    {
        SpawnProjectile();
        AddRecoil();
        ApplyKnockback();
        shakeSource.GenerateImpulse(Vector2.one);

        nextFireTime = Time.time + 60f / firerate;
    }

    private void SpawnProjectile()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            var projectilePosition = muzzle.position;
            var projectileRotation = Quaternion.Euler(0, 0, Random.Range(-spray, spray)) * muzzle.rotation;

            var projectile = Instantiate(projectilePrefab, projectilePosition, projectileRotation);
        }
    }

    private void AddRecoil()
    {
        recoilVelocity += Random.Range(-recoilAmount, recoilAmount);
    }

    private void ApplyKnockback()
    {
        rigidbody.velocity += Vector2.right * FacingDirection * -knockback;
    }
}
