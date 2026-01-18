using UnityEngine;

namespace Coop.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Movement")] public float MoveSpeed = 5f;
        public float CrawlSpeedFactor = 0.3f;
        public float Gravity = -9.81f;

        [Header("Camera")] public float MouseSensitivity = 2f;
    }
}