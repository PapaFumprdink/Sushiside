using UnityEngine;
using Cinemachine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CinemachineTargetGroup))]
public class PlayerTargetGroup : MonoBehaviour
{
    public static PlayerTargetGroup Instance;

    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private float playerRadius;

    private void Start()
    {
        Instance = this;

        if (!targetGroup) targetGroup = GetComponent<CinemachineTargetGroup>();

        foreach (var player in FindObjectsOfType<PlayerControls>())
        {
            targetGroup.AddMember(player.transform, 1f, playerRadius);
        }
    }
}
