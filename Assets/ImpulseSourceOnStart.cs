using UnityEngine;
using Cinemachine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class ImpulseSourceOnStart : MonoBehaviour
{
    private void Start()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(Vector3.one);
    }
}
