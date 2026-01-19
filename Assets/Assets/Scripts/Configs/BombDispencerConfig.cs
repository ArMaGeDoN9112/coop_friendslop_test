using UnityEngine;

namespace Coop.Configs
{
    [CreateAssetMenu(fileName = "BombDispencerConfig", menuName = "Configs/BombDispencerConfig")]
    public class BombDispencerConfig : ScriptableObject
    {
        [field: SerializeField] public float SpawnDelay { get; private set; } = 1.0f;
        [field: SerializeField] public AudioClip UsageSound { get; private set; }
        [field: SerializeField] public string InteractionHintText { get; private set; }
    }
}