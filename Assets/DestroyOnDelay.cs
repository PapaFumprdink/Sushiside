using UnityEngine;

[DisallowMultipleComponent]
public class DestroyOnDelay : MonoBehaviour
{
    [SerializeReference] private float delay = 2f;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}
