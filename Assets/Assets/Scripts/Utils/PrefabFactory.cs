using UnityEngine;
using Zenject;

namespace Coop.Utils
{
    public class PrefabFactory : IPrefabFactory
    {
        private readonly DiContainer _container;

        public PrefabFactory(DiContainer container) => _container = container;

        public GameObject Create(GameObject prefab, Transform parent, Vector3? position = null,
            Quaternion? rotation = null)
        {
            if (position == null && rotation == null) return _container.InstantiatePrefab(prefab, parent);

            return _container.InstantiatePrefab(prefab, position ?? Vector3.zero, rotation ?? Quaternion.identity,
                parent);
        }
    }
}