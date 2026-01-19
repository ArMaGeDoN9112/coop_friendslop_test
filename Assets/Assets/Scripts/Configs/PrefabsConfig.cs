using System.Collections.Generic;
using UnityEngine;

namespace Coop.Configs
{
    [CreateAssetMenu(fileName = "PrefabsConfig", menuName = "Configs/PrefabsConfig")]
    public class PrefabsConfig : ScriptableObject
    {
        [field: SerializeField] public List<GameObject> PrefabsToRegister { get; private set; }
    }
}