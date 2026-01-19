using UnityEngine;

namespace Coop.Configs
{
    [CreateAssetMenu(fileName = "BombConfig", menuName = "Configs/BombConfig")]
    public class BombConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject BombPrefab { get; private set; }
        [field: SerializeField] public float TimeToExplode { get; private set; } = 3f;
        [field: SerializeField] public float ExplosionRadius { get; private set; } = 5f;
        [field: SerializeField] public GameObject ExplosionVFXPrefab { get; private set; }
        [field: SerializeField] public AudioClip SoundEffect { get; private set; }
    }
}