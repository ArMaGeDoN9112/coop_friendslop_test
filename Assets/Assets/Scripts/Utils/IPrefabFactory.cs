using UnityEngine;

namespace Coop.Utils
{
    public interface IPrefabFactory
    {
        GameObject Create(GameObject prefab, Transform parent, Vector3? position = null, Quaternion? rotation = null);
    }
}