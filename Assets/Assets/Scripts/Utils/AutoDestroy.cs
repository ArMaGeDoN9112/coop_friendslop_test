using UnityEngine;

namespace Coop.Utils
{
    public class AutoDestroy : MonoBehaviour
    {
        [Tooltip("Время жизни эффекта (должно быть больше длины звука и частиц)")] [SerializeField]
        private float _lifetime = 3.0f;

        private void Start() => Destroy(gameObject, _lifetime);
    }
}