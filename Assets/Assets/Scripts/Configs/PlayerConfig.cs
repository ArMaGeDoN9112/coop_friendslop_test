using UnityEngine;

namespace Coop.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
        [field: SerializeField] public float CrawlSpeed { get; private set; } = 2f;
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;
        [field: SerializeField] public float MouseSensitivity { get; private set; } = 2f;
        [field: SerializeField] public float InteractDistance { get; private set; } = 10f;
        [field: SerializeField] public float AnimationDampTime { get; private set; } = 0.1f;
        [field: SerializeField] public string ReviveHint { get; private set; } = "Revive";
    }
}