using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Configs
{
    public class MirrorZenjectBridge : IInitializable
    {
        private readonly DiContainer _container;
        private readonly NetworkManager _networkManager;
        private readonly PrefabsConfig _prefabsConfig;

        public MirrorZenjectBridge(DiContainer container, NetworkManager networkManager, PrefabsConfig prefabsConfig)
        {
            _container = container;
            _networkManager = networkManager;
            _prefabsConfig = prefabsConfig;
        }

        public void Initialize()
        {
            foreach (var prefab in _prefabsConfig.PrefabsToRegister)
                NetworkClient.RegisterPrefab(prefab,
                    msg => SpawnHandler(prefab, msg),
                    UnspawnHandler);
        }

        public void Dispose()
        {
            if (!_networkManager) return;

            foreach (var prefab in _prefabsConfig.PrefabsToRegister) NetworkClient.UnregisterPrefab(prefab);
        }

        private GameObject SpawnHandler(GameObject prefab, SpawnMessage msg) =>
            _container.InstantiatePrefab(prefab, msg.position, msg.rotation, null);

        private static void UnspawnHandler(GameObject spawnedObj) => Object.Destroy(spawnedObj);
    }
}