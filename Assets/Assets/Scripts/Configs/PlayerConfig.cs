using UnityEngine;

namespace Coop.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject PlayerPrefab { get; private set; }

        [Header("Movement")]
        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 5f;

        [field: SerializeField] public float CrawlSpeedFactor { get; private set; } = 0.3f;
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;

        [Header("Camera")]
        [field: SerializeField]
        public float MouseSensitivity { get; private set; } = 2f;

        [field: SerializeField] public float InteractDistance { get; private set; } = 10f;
    }
}