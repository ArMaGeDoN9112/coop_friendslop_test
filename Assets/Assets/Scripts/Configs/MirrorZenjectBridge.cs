using Mirror;
using UnityEngine;
using Zenject;

namespace Coop.Configs
{
    public class MirrorZenjectBridge : IInitializable
    {
        private readonly DiContainer _container;
        private readonly NetworkManager _networkManager;

        public MirrorZenjectBridge(DiContainer container, NetworkManager networkManager, PlayerConfig playerConfig)
        {
            _container = container;
            _networkManager = networkManager;
        }

        public void Initialize()
        {
            foreach (var prefab in _networkManager.spawnPrefabs)
                NetworkClient.RegisterPrefab(prefab,
                    msg => SpawnHandler(prefab, msg),
                    UnspawnHandler);
        }

        private GameObject SpawnHandler(GameObject prefab, SpawnMessage msg) =>
            _container.InstantiatePrefab(prefab, msg.position, msg.rotation, null);

        private static void UnspawnHandler(GameObject spawnedObj) => Object.Destroy(spawnedObj);
    }
}